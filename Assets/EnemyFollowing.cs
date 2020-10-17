using UnityEngine;

public class EnemyFollowing : MonoBehaviour
{
    [Range(0.0f,1.0f)] public float speedRate;
    public Movement followingPlayer;
    Vector2 direction;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        direction = Vector3.Normalize(followingPlayer.transform.position - transform.position);
        transform.Translate(direction * speedRate * followingPlayer.m_moveSpeed * Time.deltaTime);
    }
}
