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

    public List<EEventFlag> activeFlags;
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
        gameData.sceneName = "Gameplay";
        gameData.playerPosition = CharacterManager.Instance.transform.position;
        gameData.activeFlags = new List<EEventFlag>();
    }

    public void ApplyData()
    {
        Debug.Log("GetData");
        CharacterManager.Instance.currentJavelinAmmo = gameData.ammo;
        CharacterManager.Instance.transform.position = gameData.playerPosition;
    }
}
