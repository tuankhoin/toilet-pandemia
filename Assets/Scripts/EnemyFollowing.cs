using UnityEngine;

public class EnemyFollowing : MonoBehaviour
{
    [Range(0.0f,1.0f)] public float speedRate;
    public PlayerMovement followingPlayer;
    public float distance = 5.0f; 

    // Update is called once per frame
    void Update()
    {
        float d = Vector3.Distance(followingPlayer.transform.position, transform.position);
        if (d < distance)
        {
            transform.LookAt(followingPlayer.transform.position);
            transform.Translate(Vector3.forward * speedRate * followingPlayer.speed * Time.deltaTime);
            transform.Rotate(new Vector3(0,90,0));
        }

    }
}
