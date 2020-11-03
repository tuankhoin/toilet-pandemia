using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [SerializeField]
    public GameObject FireBall;

    public float fireRate = 1f;
    float nextFire;
    EnemyBehavior e;
    float distance;

    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time;
        e = GetComponent<EnemyBehavior>();
        distance = e.distance;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player and karen are close to each other on the same elevation
        float d = Vector3.Distance(e.followingPlayer.transform.position, transform.position);
        float deltaHeight = Mathf.Abs(transform.position.y-e.followingPlayer.transform.position.y);
        if (d < distance && deltaHeight < 5) {
            CheckIfTimeToFire();
        }
    }
    
    // Shoot fireball after a specified period of time
    public void CheckIfTimeToFire()
    {
        if(Time.time > nextFire)
        {
            Instantiate(FireBall, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
    } 
}
