using UnityEngine;

public class Turret : EnemyBehavior
{
    // Update is called once per frame
    void Update()
    {
        float d = Vector3.Distance(followingPlayer.transform.position, transform.position);
        float deltaHeight = Mathf.Abs(transform.position.y-followingPlayer.transform.position.y);
        if (d < distance && deltaHeight < 5)
        {
            transform.LookAt(followingPlayer.transform.position);
            transform.Rotate(new Vector3(0,90,0));
            if (!audioSource.isPlaying){
                audioSource.Play();
            }
        }
        else if (audioSource.isPlaying) {
            audioSource.Stop();
        } else {
            transform.Rotate(new Vector3(0,1,0));
        }
    }
}
