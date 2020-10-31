using UnityEngine;

public class EnemyLock : MonoBehaviour
{
    EnemyBehavior[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }
}
