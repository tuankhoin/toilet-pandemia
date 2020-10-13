using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour

{
    void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }
}
