using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Abstract class for objects that are spawned randomly on the map
// Specifically in this game: karens and bonuses
public class randomSpawn : MonoBehaviour
{
    public Player player;

    // Start is called before the first frame update
    protected void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        SetPosition();
    }

    // Set a new random postition for the object from given set of locations
    public void SetPosition()
    {
        int index = Random.Range(0, Global.spawnLocations.Length);
        transform.position = Global.spawnLocations[index].transform.position;
    }
}
