using UnityEngine;

public class BulletBehaviour : MonoBehaviour

{
    void OnCollisionEnter(Collision other) {
        Destroy(gameObject);
    }
}
