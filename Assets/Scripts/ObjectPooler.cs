using UnityEngine;
using System.Collections.Generic;

// Class type containing information of each object included in the pool
[System.Serializable] public class ObjectPoolItem {
    public string name;
    public GameObject objectToPool;
    public float amountToPoolEachLevel;
    public bool shouldExpand;
}

public class ObjectPooler : MonoBehaviour {

    public static ObjectPooler SharedInstance;
    public List<ObjectPoolItem> itemsToPool;
    public List<GameObject> pooledObjects;

	void Awake() {
        // Make pool accessible globally
		SharedInstance = this;
	}

	// Use this for initialization
    void Start () {
        pooledObjects = new List<GameObject>();
        foreach (ObjectPoolItem item in itemsToPool) {
            // Initialize the number of items required for the current level
            for (int i = 0; i < Mathf.FloorToInt(item.amountToPoolEachLevel * Player.SharedInstance.level); i++) {
                
                GameObject obj = (GameObject)Instantiate(item.objectToPool);

                // Only hide fireball, the rest appears with new level
                if (item.objectToPool.tag == "Fireball") obj.SetActive(false);
                else obj.SetActive(true);
                
                // Add to storage pool as well
                pooledObjects.Add(obj);
            }
        }
    }
	
    // Returns an object from the pool to be activated
    public GameObject GetPooledObject(string tag) {
        // Search in the pool to see if there is any available object left
        for (int i = 0; i < pooledObjects.Count; i++) {
            if (!pooledObjects[i].activeInHierarchy && pooledObjects[i].tag == tag) 
            return pooledObjects[i];
        }

        // If there is none available left, just make a new one
        foreach (ObjectPoolItem item in itemsToPool) {
            if (item.objectToPool.tag == tag) {
                // Note: If there is limited capacity requirement, then skip this process and return null
                if (item.shouldExpand) {
                    GameObject obj = (GameObject)Instantiate(item.objectToPool);
                    obj.SetActive(true);
                    pooledObjects.Add(obj);
                    return obj;
                }
            }
        }
        return null;
    }
}
