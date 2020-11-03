using UnityEngine;

public class EnemyFollowing : EnemyBehavior
{
    [Range(0.0f,1.0f)] public float speedRate;
    public bool isFollowing = false;


    // Update is called once per frame
    void Update()
    {
        // Check if player and karen are close to each other on the same elevation
        float d = Vector3.Distance(followingPlayer.transform.position, transform.position);
        float deltaHeight = Mathf.Abs(transform.position.y-followingPlayer.transform.position.y);
        if (d < distance && deltaHeight < 5)
        {
            isFollowing = true; // Start stalking player
        }
        if (isFollowing)
        {
            // During stalking: always follow player...
            transform.LookAt(followingPlayer.transform.position);
            transform.Translate(Vector3.forward * speedRate * followingPlayer.speed * Time.deltaTime);
            transform.Rotate(new Vector3(0,90,0));
            // ... and make complaint
            if (!audioSource.isPlaying) audioSource.Play();
        } else if (audioSource.isPlaying) {
            audioSource.Stop();
        } else {
            // When neutral, move back and forth to patrol
            transform.Translate(new Vector3(0.5f,0,0)
                                *Mathf.Sin(Time.time)
                                *speedRate
                                *followingPlayer.speed
                                *Time.deltaTime);
        }

    }
}
