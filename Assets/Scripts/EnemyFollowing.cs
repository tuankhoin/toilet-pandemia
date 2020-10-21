using UnityEngine;

public class EnemyFollowing : MonoBehaviour
{
    [Range(0.0f,1.0f)] public float speedRate;
    public PlayerMovement followingPlayer;
    public Transform other;
    public float distance = 5.0f; 

    private bool isFollowing = false;

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(other.position, transform.position);
        if (dist < distance)
        {
            isFollowing = true;
        }
        if (isFollowing)
        {
            transform.LookAt(followingPlayer.transform.position);
            transform.Translate(Vector3.forward * speedRate * followingPlayer.speed * Time.deltaTime);
        }

    }
}
