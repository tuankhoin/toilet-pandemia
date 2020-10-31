using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : randomSpawn
{
    public PlayerMovement followingPlayer;
    public float distance = 25.0f; 
    protected AudioSource audioSource;

    // Start is called before the first frame update
    void Start () {
        audioSource = GetComponent<AudioSource>();
        base.SetPosition();
    }
    
}
