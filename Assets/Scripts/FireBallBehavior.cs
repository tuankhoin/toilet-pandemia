using UnityEngine;

public class FireBallBehavior : MonoBehaviour
{
    public float speed = 12f;
    public GameObject explosion;
    public float existTime = 10f;
    Rigidbody rb;
    GameObject target;
    Vector3 moveDirection;
    float time;
    
    // Start is called before the first frame update
    void Start()
    {
        Initiate();
    }

    public void Initiate() {
        rb = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Player");
        moveDirection = (target.transform.position - transform.position).normalized * speed;
        rb.velocity = new Vector3(moveDirection.x, moveDirection.y, moveDirection.z);
        time = Time.time;
    }

    // Update is called once per frame
    void Update () {
        if (Time.time-time > existTime) gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")){
            gameObject.SetActive(false);
            Instantiate(explosion, transform.position,transform.rotation);
        }       
    }
}
