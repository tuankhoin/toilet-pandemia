using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    //public Player player;

    void Start()
    {
        //Debug.Log(Global.overallScore);
        if (Global.overallScore > Global.maxScore){
            Global.maxScore = Global.overallScore;
        }
                
        scoreText.text = "SCORE: " + Global.overallScore.ToString()
        + "\n" + "MAX SCORE: " + Global.maxScore.ToString();
    }
    public void BackToMenu() {
        SceneManager.LoadScene("MenuScreen");
    }

    public void Quit() {
        Application.Quit();
    }
}
