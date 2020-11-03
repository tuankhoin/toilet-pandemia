using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HolyVaccine : MonoBehaviour
{
    public int awardPoints = 45;
    public int healthChange = 500;
    public float rotateSpeed;
    public Player player;

    void Update() {
        transform.Rotate(new Vector3(0,1,0), rotateSpeed);
    }

    void OnTriggerEnter(Collider collider) {
        // Works like a normal bonus, but value varies with level
        if (collider.gameObject.CompareTag("Player")){            
            gameObject.SetActive(false);
            player.ChangeHealth(healthChange);
            player.score += awardPoints*player.level;
        }
    }
}
