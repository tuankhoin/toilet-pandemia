using UnityEngine;

public class Turret : EnemyBehavior
{
    // Update is called once per frame
    void Update()
    {
        float d = Vector3.Distance(followingPlayer.transform.position, transform.position);
        if (d < distance)
        {
            transform.LookAt(followingPlayer.transform.position);
            transform.Rotate(new Vector3(0,90,0));
        }
    }
}
