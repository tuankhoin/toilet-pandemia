using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame() {
        GameObject playing = GameObject.FindGameObjectWithTag("Soundtrack");
        Destroy(playing); 
        SceneManager.LoadScene("First Round");
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
