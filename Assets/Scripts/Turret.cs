using UnityEngine;

public class Turret : EnemyBehavior
{
    AudioSource audioSource;

    void Start() {
        audioSource = GetComponent<AudioSource>();
    }
    // Update is called once per frame
    void Update()
    {
        float d = Vector3.Distance(followingPlayer.transform.position, transform.position);
        if (d < distance)
        {
            transform.LookAt(followingPlayer.transform.position);
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
