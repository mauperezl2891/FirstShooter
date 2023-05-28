using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public int scoreToWin;
    public int currentScore;

    public bool gamePaused;

    // instance
    public static GameManager instance;

    void Awake()
    {
        // set instance
        instance = this;
    }

    void Start()
    {
        Time.timeScale = 1.0f;
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))    
            TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        gamePaused = !gamePaused;
        Time.timeScale = gamePaused == true ? 0.0f : 1.0f;

        Cursor.lockState = gamePaused == true ? CursorLockMode.None : CursorLockMode.Locked;

        // toggle pause menu
        GameUi.instance.TogglePauseMenu(gamePaused);
    }

    public void AddScore(int score)
    {
        currentScore += score;

        GameUi.instance.UpdateScoreText(currentScore);

        // have we won
        if (score >= scoreToWin)
            WinGame();
    }

    void WinGame()
    {
        GameUi.instance.setEndGameScreen(true, currentScore);
        Time.timeScale = 0.0f;
        gamePaused = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void LoseGame()
    {
        // set end Game screen 
        GameUi.instance.setEndGameScreen(false, currentScore);
        Time.timeScale = 0.0f;
        gamePaused = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
