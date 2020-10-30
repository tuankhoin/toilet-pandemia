using UnityEngine;

public class Collected : MonoBehaviour
{
    public Player player;
    public int awardPoints = 100;
    public int healthChange = 100;
    public float rotateSpeed;
    public GameObject ground;
    MeshRenderer rend;


    // Start is called before the first frame update
    void Start () {
        rend = ground.GetComponent<MeshRenderer>();
        Vector3 newPos = setPos();
        transform.position = newPos;
    }

    void Update() {
        transform.Rotate(new Vector3(0,1,0), rotateSpeed);
    }
    void OnTriggerEnter(Collider collider) {

        if (collider.gameObject.CompareTag("Player")){
            
            Vector3 newPos = setPos();
            this.transform.position = newPos;
            player.ChangeHealth(healthChange);
            player.score += awardPoints;
            //Debug.Log(player.score);
        }
    }

    public Vector3 generatePos(float minX, float minZ, float maxX, float maxZ) {
        float xVal = Random.Range(minX,maxX);
        float zVal = Random.Range(minZ,maxZ);  
        return new Vector3(xVal,this.transform.position.y,zVal);
    }

    public Vector3 setPos() {

        float left = rend.bounds.min.x;
        float right = rend.bounds.max.x;
        float up = rend.bounds.min.z;
        float down = rend.bounds.max.z;

        Vector3 newPos = generatePos(left,down,right,up);
        while (!isValid(newPos)) {
            newPos = generatePos(left,down,right,up);
        }
        //Debug.Log(newPos);
        return newPos;
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
