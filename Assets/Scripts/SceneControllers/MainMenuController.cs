﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame() {
        SceneManager.LoadScene("DemoMap");
    }

    public void OpenInstructions() {
        SceneManager.LoadScene("Instructions");
    }

    public void OpenOptions() {
        SceneManager.LoadScene("Options");
    }

    public void Quit() {
        Application.Quit();
    }
}
