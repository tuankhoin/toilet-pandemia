using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : randomSpawn
{
    public PlayerMovement followingPlayer;
    public float distance = 25.0f; 
    protected AudioSource audioSource;

    // Start is called before the first frame update
    new void Start () {
        base.Start();
        followingPlayer = player.GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }
    
}
