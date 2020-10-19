using UnityEngine;
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
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        pausingScreen.SetActive(true);
        isPaused = true;
    } 
    public void ContinueGame()
    {
        Time.timeScale = 1f;
        pausingScreen.SetActive(false);
        isPaused = false;
    }

    public void Quit() {
        Application.Quit();
    }
}
