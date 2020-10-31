using UnityEngine;

public class EnemyLock : MonoBehaviour
{
    GameObject[] enemies;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        if (enemies.Length == 0) {
            gameObject.SetActive(false);
        } else if (enemies.Length != 0 && !gameObject.activeSelf) {
            gameObject.SetActive(true);
        }
    }
}
