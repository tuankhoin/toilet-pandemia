using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI levelText;

    // Update is called once per frame
    void Update()
    {
        if (player.isCountDown) {
            levelText.text = "NEXT LEVEL IN " + Mathf.Ceil(player.timeLeft).ToString();
        } else {
            levelText.text = "LEVEL " + player.level.ToString(); 
        }       
    }
}
