using UnityEngine;

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
    public GameObject battleCry;

    public int scoreGain = 10;

    // Start is called before the first frame update
    void Start () {
        health = fullHealth;
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }
    
    public void takeDamage(float amount) {
        health -= amount;
        if (health <= 0f) {
            die();
        } else {
            GameObject parent = gameObject.transform.parent.gameObject;
            EnemyFollowing ef = parent.GetComponent<EnemyFollowing>();
            if (ef != null) {
                ef.isFollowing = true;
            }
        }
    }

    void die() {
        GameObject parent = gameObject.transform.parent.gameObject;
        parent.SetActive(false);
        health = fullHealth;
        Instantiate(explosion, transform.position, transform.rotation);
        Instantiate(battleCry, transform.position, transform.rotation);
        player.score += scoreGain;
        player.targets = GameObject.FindObjectsOfType<EnemyBehavior>();
        
        EnemyFollowing ef = parent.GetComponent<EnemyFollowing>();
        if (ef != null) {
            ef.isFollowing = false;
        }
    }
}
