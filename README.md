# Toilet Pandemia

**To-Do List**

- [ ] Brief explanation of the game
- [ ] How to use it (especially the user interface aspects)
- [ ] How you modelled objects and entities
- [ ] How you handled the graphics pipeline and camera motion
- [ ] Descriptions of how the shaders work
- [ ] Description of the querying and observational methods used, including: description of the participants (how many, demographics), description of the methodology (which techniques did you use, what did you have participants do, how did you record the data), and feedback gathered.
- [ ] Document the changes made to your game based on the information collected during the evaluation.
- [ ] A statement about any code/APIs you have sourced/used from the internet that is not your own.
- [ ] A description of the contributions made by each member of the group.



[TOC]

## Team Members

|       Name        |
| :---------------: |
|   Angus Hudson    |
|    Khoi Nguyen    |
| Luu Hoang Anh Huy |
|    Hoang Long     |

## Game Explanation and Gameplay
### **Game Explanation**

Our game is a first person shooter (FPS), based in a post-apocalyptic world where COVID-19 has ravaged the world's population. You assume the role of an average manager, intent on locating and distributing the vaccine to finally put an end to the pandemic. However this vaccine is held in a nearby shopping center, defended by a horde of Karens who want nothing more than to see the world burn, having succumbed to the frustrations of state-enforced lockdown long ago. 

Your objective, to enter the shopping center, and collect critical supplies for the residents in your community, all the while doing the following:

1. Avoiding incineration at the hands of the Karens' powerful fire attacks
2. Maintaining an appropriate 1.5m social distance, or else risk contracting COVID-19 from the Karens themselves
3. Surviving long enough to discover the super vaccine, and taking it when it appears

Points are accrued for gathering supplies, defeating Karens, and surviving levels. Health packs will also randomly spawn, that will allow the player to recover any lost health. The game takes on a classic arcade 'survival' format, that is, the player plays until he/she finally falls to the Karen hordes, an inevitability since each level rises in difficulty to eventually impossible scenarios.

### **Gameplay**

#### **<u>Controls</u>**

*W/A/S/D*- Character Movement

*Space*- Jump

*Left-Mouse*- Shoot

#### **<u>User Interface</u>**

#### Menu

#### Game Over

#### Gameplay

*Health Bar*

*Scoring System*

*Level Count*

*Hints*

[Insert UI screenshot with captions explaining the different features on-screen]

## Modelling Objects and Entities

### Object Pooling

In order to maintain efficiency for CPU and avoid continuous calls of `Instanstiate()` and `Destroy()`, an object pool is created in order to keep game objects reusable. The common mechanisim goes as follows (based on [pooling tutorial by Mark Placzek](https://www.raywenderlich.com/847-object-pooling-in-unity) ):

* A pool of chosen data structure (in this project: `List`) type is created to store objects of a specified number.
```C#
    // How a pooled object's information are stored - ObjectPool.cs
    [System.Serializable] public class ObjectPoolItem {
        public string name;                 // Object name
        public GameObject objectToPool;     // Prefab of object
        public float amountToPoolEachLevel; // How many to expand each level
        public bool shouldExpand;           // Indicates no limit in capacity
    }

    public List<ObjectPoolItem> itemsToPool;    // Information storage
    public List<GameObject> pooledObjects;      // Object pool data structure
```

```C#
    // How a pool is initiated - ObjectPool.cs

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
```

* Every time in need, instead of calling `Instanstiate`, system will choose an inactive item in the pool data structure and activate it.
```C#
	// How objects are retrieved on new level rather than Instanstiating - Player.cs 
	void SpawnNewLevel() {

		// Look through each pool of item
		foreach (ObjectPoolItem item in ObjectPooler.SharedInstance.itemsToPool) {
			string tag = item.objectToPool.tag;

			// Fireball is only stored for boss karens only, not with levels
			if (tag == "Fireball") continue;

			// Pick items with needed quantity (by level), and set to be active in map
            for (int i = 0; i < Mathf.FloorToInt(item.amountToPoolEachLevel * level); i++) {
                GameObject obj = ObjectPooler.SharedInstance.GetPooledObject(tag);
				if (obj != null) {
                    // Allocate a spawn position, and spawn object from there
					obj.GetComponent<randomSpawn>().SetPosition();
                	obj.SetActive(true);
				}
            }
        }
	}
```

* In case of no available object in the pool left, system can either stop if there is a capacity limit, or `Instanstiate` another available object to put to list and later used in the future.
```C#
    // How an object is retrieved from the pool - ObjectPool.cs

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
                // Note: If there is limited capacity requirement, 
                // then skip this process and return null
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
```
* When ending functionality, rather than `Destroy`, system will deactivate the object ( `GameObject.SetActive(false)` ) and put it back to the data structure for later usage.

Currently in the game, the object pool is being used on the following objects that will require the most amount of `Instanstiate` if not using pool:
* Karens
* Bonus items
* Fireball shot by Karens




### Karen Control

#### Following Player

#### Shooting Fireballs

### Level Switching & Vaccine

****

## Graphics and Camera

## Shaders and Particles

Evaluate on this very carefully guys! They mostly care abt this and evaluation!
* How shader works (and show which variables does what)
* How it is efficient to CPU

### Outline Shader

### Half-tone Shader

### Additive Blending for Particles

## Evaluation Techniques

As part of the development of this game, two evaluation techniques were utilized to gather feedback from five external participants and improve the game. One querying method, 'cooperative evaluation', and one observational method, 'questionnaire', made up these two techniques.

We felt that these two techniques were very synergistic, since cooperative evaluation involves an ongoing dialogue during gameplay, effectively capturing player thoughts during a playthrough, and a questionnaire is completed after gameplay, after the player has had ample chance to reflect. This meant that we would gather useful insights at all stages of the player experience. Both were also practical given the current climate, since both had zero requirements for face-to-face contact.

For cooperative evaluation, the test user entered a 1-on-1 Zoom call with a member of the team, and would share their screen. During gameplay, the test user was invited to share any thoughts they had when playing the game, with emphasis on zero judgement for any comments made, which encouraged an open dialogue between the team member and test user. When the user was silent, the user was left to their own device.

For the questionnaire, the test user was given a link to an online questionnaire roughly 10-15 minutes after completion of the game. See below for a link to the questionnaire:

https://www.surveymonkey.com/r/2ZJDMKM

The intent of this questionnaire was to uncover any core gameplay issues that users felt detracted from the quality of the game, and also prompted for any new features the user would like to see.

## External Code/APIs

* Long's Supermarket assets
* Minecraft asset
* C# code for shader 

## Team Contributions


