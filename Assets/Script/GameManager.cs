using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static SaveSystem saveSystem;

    public GameObject player;

    public List<EHealthUpgradeFlag> aquiredHealthUpgrades = new();

    public List<EAmmoUpgradeFlag> aquiredAmmoUpgrades = new();

    public List<EEventFlag> activeEventFlags = new();

    public List<ECharacterShape> usableShapes = new();

    // Used to initialize maxhealth when loading
    public static int startingHealth = 6;

    private IEnumerator loadCoroutine;

    void Awake()
	{
		if (instance != null)
		{
            Destroy(gameObject);
		}
        instance = this;
        DontDestroyOnLoad(gameObject);

        saveSystem = GetComponent<SaveSystem>();
	}

	// Start is called before the first frame update
	void Start()
    {
        //LoadGame();
        loadCoroutine = LoadGameCoroutine();
        LoadGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string name)
	{
        SceneManager.LoadSceneAsync(name);
    }

    public void SaveGame()
    {
        GameData gameData = new GameData();
        gameData.health = CharacterManager.Instance.HealthManager.GetHealth();
        gameData.ammo = CharacterManager.Instance.currentJavelinAmmo;
        gameData.sceneName = SceneManager.GetActiveScene().name;
        gameData.playerPosition = CharacterManager.Instance.transform.position;
        gameData.activeEventFlags = activeEventFlags;
        gameData.activeHealthFlags = aquiredHealthUpgrades;
        gameData.activeAmmoFlags = aquiredAmmoUpgrades;
        gameData.usableShapeFlags = usableShapes;
        gameData.currentShape = CharacterManager.Instance.ShapeController.CharacterShape;

        saveSystem.WriteFile(gameData);
    }

    public void LoadGame()
    {
        StopCoroutine(loadCoroutine);
        loadCoroutine = LoadGameCoroutine();
        StartCoroutine(loadCoroutine);
    }

    private IEnumerator LoadGameCoroutine()
    {
        float gravityScale = CharacterManager.Instance.rb.gravityScale;
        LoadData();
        CharacterManager.Instance.rb.bodyType = RigidbodyType2D.Static;
        // Wait until the level finish loading
        yield return new WaitForSeconds(1f);
        // Wait a frame so every Awake and Start method is called
        yield return new WaitForEndOfFrame();
        CharacterManager.Instance.rb.bodyType = RigidbodyType2D.Dynamic;
        yield return 0;
    }

    public void LoadData()
    {
        Debug.Log("GetData");
        GameData gameData = saveSystem.ReadFile();
        if (gameData != null)
        {
            Debug.Log("ApplyData");

            if (!SceneManager.GetActiveScene().name.Equals(gameData.sceneName))
                LoadScene(gameData.sceneName);
            CharacterManager.Instance.transform.position = gameData.playerPosition;

            GameManager.instance.activeEventFlags = gameData.activeEventFlags;
            GameManager.instance.aquiredHealthUpgrades = gameData.activeHealthFlags;
            GameManager.instance.aquiredAmmoUpgrades = gameData.activeAmmoFlags;
            GameManager.instance.usableShapes = gameData.usableShapeFlags;
            CharacterManager.Instance.SetShape(gameData.currentShape);

            CharacterManager.Instance.HealthManager.SetMaxHealth
                (GameManager.startingHealth + (gameData.activeHealthFlags.Count * 2));
            CharacterManager.Instance.HealthManager.SetHealth(gameData.health);

            CharacterManager.Instance.SetMaxAmmo(gameData.activeAmmoFlags.Count * 2);
            CharacterManager.Instance.SetAmmo(gameData.ammo);
        }
    }

    public void AddFlag(EEventFlag name)
    {
        if (!activeEventFlags.Contains(name))
            activeEventFlags.Add(name);
    }
    public void AddFlag(EHealthUpgradeFlag name)
    {
        if (!aquiredHealthUpgrades.Contains(name))
            aquiredHealthUpgrades.Add(name);
    }
    public void AddFlag(EAmmoUpgradeFlag name)
    {
        if (!aquiredAmmoUpgrades.Contains(name))
            aquiredAmmoUpgrades.Add(name);
    }
    public void AddShape(ECharacterShape name)
    {
        if (!usableShapes.Contains(name))
            usableShapes.Add(name);
    }

    public void SetPlayer(GameObject _player)
	{
        this.player = _player;
	}
}
