using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSpawn : MonoBehaviour
{
    GameObject [] spawnLocations;
    //
    protected void InitSpawn()
    {
        spawnLocations = GameObject.FindGameObjectsWithTag("Spawn");
        SetPosition();
    }

    //
    void SetPosition()
    {
        int index = Random.Range(0, spawnLocations.Length);
        transform.position = spawnLocations[index].transform.position;
    }
}
