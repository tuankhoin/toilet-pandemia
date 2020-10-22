using UnityEngine;

public class EnemyFollowing : EnemyBehavior
{
    [Range(0.0f,1.0f)] public float speedRate;
    AudioSource audioSource;
    private bool isFollowing = false;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float d = Vector3.Distance(followingPlayer.transform.position, transform.position);
        if (d < distance)
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
        else {
            audioSource.Stop();
        }

    }
}
