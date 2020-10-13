using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject bullet;
    public float speed = 100f;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Comma)) {
            GameObject instBullet = Instantiate(bullet, transform.position + new Vector3(0, 0.5f, 0) + transform.forward, transform.rotation * Quaternion.Euler(0,0,150)) as GameObject;
            Rigidbody instBulletRigidBody = instBullet.GetComponent<Rigidbody>();
            instBulletRigidBody.AddRelativeForce(new Vector3(0,0,speed));
        }
    }
}
