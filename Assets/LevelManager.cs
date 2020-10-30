using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI levelText;

    // Update is called once per frame
    void Update()
    {
        levelText.text = "LEVEL " + player.level.ToString();
    }
}
