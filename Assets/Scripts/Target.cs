using UnityEngine;
using UnityEngine.UI;

/**
* Each body part of a boss is attatched to its own health and score
* To kill an enemy, only need to destroy 1 of its body parts
* Headshot gains more points, of course
*/
public class Target : MonoBehaviour
{
    public Player player;
    public float fullHealth = 50f;
    float health;
    public GameObject explosion;
    public AudioSource battleCry;
    public RectTransform healthBar;

    public int scoreGain = 10;

    // Start is called before the first frame update
    void Start () {
        health = fullHealth;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();


        healthBar = this.gameObject.transform.parent.GetChild(this.gameObject.transform.parent.childCount - 1).GetChild(0).GetChild(0).gameObject.GetComponent<RectTransform>();
        healthBar.sizeDelta = new Vector2(200, healthBar.sizeDelta.y);

        // Each Karen type has their own sounds
        GameObject karen = transform.parent.gameObject;
        if (karen.tag == "Boss") {
            battleCry = GameObject.Find("GetOnTheBeers").GetComponent<AudioSource>();
        } else if (karen.tag == "Turret") {
            battleCry = GameObject.Find("ThatsABreach").GetComponent<AudioSource>();
        } else if (karen.tag == "Enemy") {
            battleCry = GameObject.Find("ShoutingAudio").GetComponent<AudioSource>();
        }
    }

    
    
    // Take away health and check if it reaches 0
    public void takeDamage(float amount) {
        health -= amount;
        //Debug.Log(health);
        healthBar.sizeDelta = new Vector2(health * 200/fullHealth, healthBar.sizeDelta.y);
        if (health <= 0f) {
            die();
        } else {
            GameObject parent = gameObject.transform.parent.gameObject;
            EnemyFollowing ef = parent.GetComponent<EnemyFollowing>();
            if (ef != null) ef.isFollowing = true;
        }
        ;
    }

    void die() {
        if (!battleCry.isPlaying) battleCry.Play();
        
        // Eliminate whole character, not just body part
        GameObject parent = gameObject.transform.parent.gameObject;
        parent.SetActive(false);

        // Player's effect
        Instantiate(explosion, transform.position + Vector3.up, transform.rotation);
        player.score += scoreGain;
        player.targets = GameObject.FindObjectsOfType<EnemyBehavior>();
        
        // Reset information for next reuse
        health = fullHealth;
        EnemyFollowing ef = parent.GetComponent<EnemyFollowing>();
        if (ef != null) ef.isFollowing = false;
    }
}
