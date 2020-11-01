using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI karenText;

    // Update is called once per frame
    void Update()
    {
        if (player.isCountDown) {
            levelText.text = "NEXT LEVEL IN " + Mathf.Ceil(player.timeLeft).ToString();
            karenText.text = "GET THE VACCINE BEFORE IT DISAPPEARS";
        } else {
            levelText.text = "LEVEL " + player.level.ToString(); 
            karenText.text ="KARENS REMAINING: " + player.targets.Length.ToString();
        }       
    }
}
