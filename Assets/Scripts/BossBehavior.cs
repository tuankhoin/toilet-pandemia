using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    [SerializeField]
    public GameObject FireBall;

    public float fireRate = 1f;
    float nextFire;
    EnemyFollowing e;
    float distance;
    
    // Start is called before the first frame update
    void Start()
    {
        nextFire = Time.time;
        e = GetComponent<EnemyFollowing>();
        distance = e.distance;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(e.followingPlayer.transform.position,
             transform.position) < distance) {
            CheckIfTimeToFire();
        }
    }
    
    public void CheckIfTimeToFire()
    {
        if(Time.time > nextFire)
        {
            Instantiate(FireBall, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
    } 
}
