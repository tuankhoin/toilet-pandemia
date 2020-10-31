using UnityEngine;

public class EnemyFollowing : EnemyBehavior
{
    [Range(0.0f,1.0f)] public float speedRate;
    private bool isFollowing = false;


    // Update is called once per frame
    void Update()
    {
        float d = Vector3.Distance(followingPlayer.transform.position, transform.position);
        float deltaHeight = Mathf.Abs(transform.position.y-followingPlayer.transform.position.y);
        if (d < distance && deltaHeight < 5)
        {
            isFollowing = true;
        }
        if (isFollowing)
        {
            transform.LookAt(followingPlayer.transform.position);
            transform.Translate(Vector3.forward * speedRate * followingPlayer.speed * Time.deltaTime);
            transform.Rotate(new Vector3(0,90,0));
            if (!audioSource.isPlaying){
                audioSource.Play();
            }
            
        }
        else if (audioSource.isPlaying) {
            audioSource.Stop();
        }

    }
}
