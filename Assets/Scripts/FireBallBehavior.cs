using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;

using UnityEngine;

public class FireBallBehavior : MonoBehaviour
{
    public float speed = 12f;
    Rigidbody rb;
    GameObject target;
    Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        moveDirection = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
        Destroy(gameObject, 10f);


    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            Destroy(gameObject);
        }       
    }
}
