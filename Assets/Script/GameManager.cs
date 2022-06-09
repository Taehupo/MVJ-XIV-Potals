using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<EHealthUpgradeFlag> aquiredHealthUpgrades;

    public List<EAmmoUpgradeFlag> aquiredAmmoUpgrades;

    public List<EEventFlag> activeEventFlags;

    public List<ECharacterShape> usableShapes;

    // Used to initialize maxhealth when loading
    public static int startingHealth = 6;

    void Awake()
	{
		if (instance != null)
		{
            Destroy(gameObject);
		}
        instance = this;
        DontDestroyOnLoad(gameObject);
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadScene(string name)
	{
        SceneManager.LoadSceneAsync(name);
    }

    public void AddFLag(EEventFlag name)
    {
        if (!activeEventFlags.Contains(name))
            activeEventFlags.Add(name);
    }
    public void AddFLag(EHealthUpgradeFlag name)
    {
        if (!aquiredHealthUpgrades.Contains(name))
            aquiredHealthUpgrades.Add(name);
    }
    public void AddFLag(EAmmoUpgradeFlag name)
    {
        if (!aquiredAmmoUpgrades.Contains(name))
            aquiredAmmoUpgrades.Add(name);
    }
}
