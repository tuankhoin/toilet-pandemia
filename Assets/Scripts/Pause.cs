﻿using UnityEngine;
public class Pause : MonoBehaviour
{
    public GameObject pausingScreen;
    public bool isPaused = false;

    public void Start(){
        pausingScreen.SetActive(false);
    }

    public void Update() {
        if(Input.GetKeyDown (KeyCode.P)) 
        {
            if (!isPaused) 
            {
                PauseGame();
            }
            else
            {
                ContinueGame();   
            }
        } 
        if(Input.GetKeyDown (KeyCode.Q)) Quit();
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        pausingScreen.SetActive(true);
        isPaused = true;
    } 
    public void ContinueGame()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        pausingScreen.SetActive(false);
        isPaused = false;
    }

    public void Quit() {
        Application.Quit();
    }
}
