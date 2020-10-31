using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSpawn : MonoBehaviour
{
    public Player player;

    // Start is called before the first frame update
    protected void Start () {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
        SetPosition();
    }
    protected void SetPosition()
    {
        int index = Random.Range(0, Global.spawnLocations.Length);
        transform.position = Global.spawnLocations[index].transform.position;
    }
}
