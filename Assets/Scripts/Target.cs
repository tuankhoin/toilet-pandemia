using UnityEngine;

/**
* Each body part of a boss is attatched to its own health and score
* To kill an enemy, only need to destroy 1 of its body parts
* Headshot gains more points, of course
*/
public class Target : MonoBehaviour
{
    public Player player;
    public float health = 50f;
    public GameObject explosion;

    public int scoreGain = 10;
    // Start is called before the first frame update
    public void takeDamage(float amount) {
        health -= amount;
        if (health <= 0f) {
            die();
        }
    }

    void die() {
        Destroy(gameObject.transform.parent.gameObject);
        Instantiate(explosion, transform.position, transform.rotation);
        player.score += scoreGain;
        player.targets = GameObject.FindGameObjectsWithTag("Enemy");
    }
}
