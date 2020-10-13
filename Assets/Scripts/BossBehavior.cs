using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [SerializeField]
    GameObject FireBall;

    public float fireRate = 1f;
    float nextFire;
    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time;
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckIfTimeToFire();
    }
    
    void CheckIfTimeToFire()
    {
        if(Time.time > nextFire)
        {
            Instantiate(FireBall, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
    } 
}
