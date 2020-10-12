using UnityEngine;

public class Collected : MonoBehaviour
{
    public Player player;
    public int awardPoints = 100;
    public int healthChange = 100;
    public GameObject wallLeft;
    public GameObject wallRight;
    public GameObject wallUp;
    public GameObject wallDown;


    // Start is called before the first frame update
    void OnTriggerEnter(Collider collider) {
        
        float left = wallLeft.transform.position.x;
        float right = wallRight.transform.position.x;
        float up = wallUp.transform.position.z;
        float down = wallDown.transform.position.z;
        

        if (collider.gameObject.CompareTag("Player")){
            //Vector3 newPos = generatePos(-3,-3,3,3);
            Vector3 newPos = generatePos(left,down,right,up);
            while (!isValid(newPos)) {
                //newPos = generatePos(-3,-3,3,3);
                newPos = generatePos(left,down,right,up);
            }
            Debug.Log(newPos);
            Instantiate(this, newPos, this.transform.rotation);
            Destroy(gameObject);
            player.ChangeHealth(healthChange);
            player.score += awardPoints;
        }
    }

    public Vector3 generatePos(float minX, float minZ, float maxX, float maxZ) {
        float xVal = Random.Range(minX,maxX);
        float zVal = Random.Range(minZ,maxZ);  
        return new Vector3(xVal,this.transform.position.y,zVal);
    }
    
    public bool isValid(Vector3 targetPos)
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        foreach(GameObject current in walls)
        {
            if(current.transform.position == targetPos)
                return false;
        }
        return true;
    }
}
