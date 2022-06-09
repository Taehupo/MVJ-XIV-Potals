using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameData
{
    public int health;

    public int ammo;

    public string sceneName;

    public Vector3 playerPosition;

    public List<EEventFlag> activeEventFlags;

    public List<EHealthUpgradeFlag> activeHealthFlags;

    public List<EAmmoUpgradeFlag> activeAmmoFlags;

    public List<ECharacterShape> usableShapeFlags;
}
public class SaveSystem : MonoBehaviour
{
    private string saveFile;
    private GameData gameData = new GameData();

    void Awake()
    {
        saveFile = Application.persistentDataPath + "/gamedata.json";
    }

     public void ReadFile()
    {
        string fileContents = File.ReadAllText(saveFile);
        gameData = JsonUtility.FromJson<GameData>(fileContents);
        ApplyData();
        Debug.Log("Read");
    }

    public void WriteFile()
    {
        GetData();
        string jsonString = JsonUtility.ToJson(gameData);
        File.WriteAllText(saveFile, jsonString);
        Debug.Log("Wrote");
    }

    public void GetData()
    {
        Debug.Log("GetData");
        gameData.health = CharacterManager.Instance.HealthManager.GetHealth();
        gameData.ammo = CharacterManager.Instance.currentJavelinAmmo;
        gameData.sceneName = "Gameplay"; // SetScene
        gameData.playerPosition = CharacterManager.Instance.transform.position;
        gameData.activeEventFlags = GameManager.instance.activeEventFlags;
        gameData.activeHealthFlags = GameManager.instance.aquiredHealthUpgrades;
        gameData.activeAmmoFlags = GameManager.instance.aquiredAmmoUpgrades;
        gameData.usableShapeFlags = GameManager.instance.usableShapes;
    }

    public void ApplyData()
    {
        Debug.Log("GetData");
        GameManager.instance.activeEventFlags = gameData.activeEventFlags;
        GameManager.instance.aquiredHealthUpgrades = gameData.activeHealthFlags;
        GameManager.instance.aquiredAmmoUpgrades = gameData.activeAmmoFlags;
        GameManager.instance.usableShapes = gameData.usableShapeFlags;

        CharacterManager.Instance.HealthManager.SetMaxHealth
            (GameManager.startingHealth + (gameData.activeHealthFlags.Count * 2));
        CharacterManager.Instance.HealthManager.SetHealth(gameData.health);

        CharacterManager.Instance.SetMaxAmmo(gameData.activeAmmoFlags.Count * 2);
        CharacterManager.Instance.SetAmmo(gameData.ammo);

        //Load Scene
        CharacterManager.Instance.transform.position = gameData.playerPosition;
    }
}
