using UnityEngine;

public class Target : MonoBehaviour
{
    public Player player;
    public float health = 50f;

    public int scoreGain = 10;
    // Start is called before the first frame update
    public void takeDamage(float amount) {
        health -= amount;
        if (health <= 0f) {
            die();
        }
    }

    void die() {
        Destroy(gameObject);
        player.score += scoreGain;
        player.targets = GameObject.FindGameObjectsWithTag("Enemy");
    }
}
