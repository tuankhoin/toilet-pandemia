using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LevelManager : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI karenText;
    public TextMeshProUGUI hintText;
    public int changeHintTime = 5;
    public List<string> hints;
    int counter;
    int index;

    // Update is called once per frame
    void Start () {
        counter = 8;
        StartCoroutine(ChangeHint());
    }
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

    IEnumerator ChangeHint () {
        while (true) {
            index = counter % hints.Count;
            hintText.text = "FACTS: " + hints[index];
            counter++;
            yield return new WaitForSeconds(changeHintTime);
        }
    }
}
