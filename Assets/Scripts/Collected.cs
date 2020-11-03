using UnityEngine;

public class Collected : randomSpawn
{
    public int awardPoints = 100;
    public int healthChange = 100;
    public float rotateSpeed;
    AudioSource collectSound;

    new void Start () {
        base.Start();
        collectSound = GameObject.FindGameObjectWithTag("CollectSound").GetComponent<AudioSource>();
    }

    // Make it rotate to look cool
    void Update() {
        transform.Rotate(new Vector3(0,1,0), rotateSpeed);
    }

    void OnTriggerEnter(Collider collider) {
        // Apply effect when player collects item
        if (collider.gameObject.CompareTag("Player")){
            base.SetPosition();
            if (!collectSound.isPlaying) collectSound.Play();
            gameObject.SetActive(false);
            player.ChangeHealth(healthChange);
            player.score += awardPoints;
        }
    }
}
