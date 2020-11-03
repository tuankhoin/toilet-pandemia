using UnityEngine;
using TMPro;

public class ScoringSystem : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI scoreText;

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "SCORE: " + player.score.ToString();
        Global.overallScore = player.score;
    }
}
