using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class randomSpawn : MonoBehaviour
{
    //
    protected void SetPosition()
    {
        int index = Random.Range(0, Global.spawnLocations.Length);
        transform.position = Global.spawnLocations[index].transform.position;
    }
}
