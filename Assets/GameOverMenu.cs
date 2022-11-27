using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenu : MonoBehaviour
{
    public GameObject gameOverMenuUI;
    public static GameOverMenu Instance;
    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }
    public bool GameIsPaused()
    {
        return PauseMenu.GameIsPaused;
    }
    public void SetPause(bool pauseState)
    {
        PauseMenu.GameIsPaused = pauseState;
    }

    public void DisplayMenu()
    {
        gameOverMenuUI.SetActive(true);
        Time.timeScale = 0f;
        SetPause(true);
    }

    public void Continue()
    {
        gameOverMenuUI.SetActive(false);
        Time.timeScale = 1f;
        SetPause(false);
        GameManager.instance.LoadGame();
    }
    public void Quit()
    {
        Application.Quit();
    }
}
