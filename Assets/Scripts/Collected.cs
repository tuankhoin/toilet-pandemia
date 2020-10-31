using UnityEngine;

public class Collected : randomSpawn
{
    public int awardPoints = 100;
    public int healthChange = 100;
    public float rotateSpeed;

    void Update() {
        transform.Rotate(new Vector3(0,1,0), rotateSpeed);
    }

    void OnTriggerEnter(Collider collider) {

        if (collider.gameObject.CompareTag("Player")){
            
            base.SetPosition();
            gameObject.SetActive(false);
            player.ChangeHealth(healthChange);
            player.score += awardPoints;
            //Debug.Log(player.score);
        }
    }
}
