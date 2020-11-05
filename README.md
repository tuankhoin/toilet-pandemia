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


## Table of contents
<details>
<summary> Click to expand
</summary>

- [Toilet Pandemia](#toilet-pandemia)
  - [Table of contents](#table-of-contents)
  - [Team Members](#team-members)
  - [Game Explanation and Gameplay](#game-explanation-and-gameplay)
    - [**Game Explanation**](#game-explanation)
    - [**Gameplay**](#gameplay)
      - [**<u>Controls</u>**](#ucontrolsu)
      - [**<u>User Interface</u>**](#uuser-interfaceu)
      - [***Menu***](#menu)
      - [***Game Over***](#game-over)
      - [***Gameplay***](#gameplay-1)
  - [Modelling Objects and Entities](#modelling-objects-and-entities)
    - [Object Pooling](#object-pooling)
    - [Karen Control](#karen-control)
      - [**Following Player**](#following-player)
      - [**Close-Range Infection**](#close-range-infection)
      - [**Shooting Fireballs**](#shooting-fireballs)
    - [Level Switching & Vaccine](#level-switching--vaccine)
  - [Graphics and Camera](#graphics-and-camera)
  - [Shaders and Particles](#shaders-and-particles)
    - [Toon Shader](#toon-shader)
      - [1. Multiple light sources:](#1-multiple-light-sources)
      - [2. Ambient Light:](#2-ambient-light)
      - [3. Specular refecltion](#3-specular-refecltion)
      - [4. Rim lighting](#4-rim-lighting)
    - [Outline Shader](#outline-shader)
    - [Half-tone Shader](#half-tone-shader)
    - [Transparency Modification Shaders](#transparency-modification-shaders)
    - [Particles](#particles)
      - [Explosion](#explosion)
      - [Additive Blending](#additive-blending)
  - [Evaluation Techniques](#evaluation-techniques)
  - [External Code/APIs](#external-codeapis)
  - [Team Contributions](#team-contributions)

</details>

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
|    Button    |      Function      |
| :----------: | :----------------: |
|  `W/A/S/D`   | Character Movement |
|   `Space`    |        Jump        |
| `Left-Mouse` |       Shoot        |
|     `R`      |       Reload       |

#### **<u>User Interface</u>**

#### ***Menu***
<p align="center">
  <img src="Gifs/mainmenu.gif" width="400" >
  <br>Main menu.
</p>

#### ***Game Over***
<p align="center">
  <img src="Gifs/gameover.gif" width="400" >
  <br>Game over.
</p>

#### ***Gameplay***

*Health Bar*

*Scoring System*

*Level Count*

*Hints*

[Insert UI screenshot with captions explaining the different features on-screen]

## Modelling Objects and Entities

### Object Modelling

**Third-Party**

To conserve time and focus on gameplay elements, many of the gameplay assets were sourced from third parties online:

- Gun sourced from the 'Sci-Fi Weapons' free pack at https://devassets.com/assets/sci-fi-weapons/
- Flashlight sourced from...
- The supermarket environment sourced from...

To ensure a consistent aesthetic for the game in spite of these different sources of objects, the toon shader (see below) was utilized for all objects.

**Custom-Made**

<u>Karens</u> 

The Karens were modelled utilizing a simple custom-made texture superimposed on a default 'Minecraft Steve' object, sourced from .... The textures were custom-made, and we felt that their utilization solely for the Karen's meant that they sharply contrasted with the rest of the game aesthetic, making them clearly identifiable to any player.

To further distinguish them, Karen's were given a fog shader (see below) of differing colors, a different size, and potentially given a particle system (see below), depending on their strength. The objective of this was to make it clear for the player the relative strengths of the different Karen's present in the game.

### Object Pooling

In order to maintain efficiency for CPU and avoid continuous calls of `Instantiate()` and `Destroy()`, an object pool is created in order to keep game objects reusable. The common mechanism goes as follows (based on [pooling tutorial by Mark Placzek](https://www.raywenderlich.com/847-object-pooling-in-unity)):

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

* Every time in need, instead of calling `Instantiate`, system will choose an inactive item in the pool data structure and activate it.
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

Currently in the game, the object pool is being used on the following objects that will require the most amount of `Instantiate` if not using pool:
* Karens
* Bonus items
* Fireball shot by Karens

<p align="center">
  <img src="Gifs/objectpool.gif" width="100" >
  <br>Pooled objects are only created once on game start, and only change active status afterwards.
</p>

### Karen Control

As the main enemy, Karens in the game has the objective to chase and infect the player.

#### **Following Player**
When the player is detected to be in sight, depending on the type of Karen, it will either follow, or aim at player
```C#
// Check if player and karen are close to each other on the same elevation - EnemyFollowing.cs

float d // Euclidean distance
= Vector3.Distance(followingPlayer.transform.position, transform.position); 

float deltaHeight // Height difference, to avoid detection between different floors
= Mathf.Abs(transform.position.y-followingPlayer.transform.position.y); 

// If in sight...
if (d < distance && deltaHeight < minimumHeightDifference) {
    // Follow or aim at player
    transform.LookAt(followingPlayer.transform.position);
    ...
}
```
<p align="center">
  <img src="Gifs/karenchasing.gif" width="400" >
  <br>A Karen detected and then chased the player.
</p>

#### **Close-Range Infection**
Of course, if the player do not keep social distancing, then health will decrease by time.
```C#
// Called every update to see if social distancing is maintained. If not, drain health - Player.cs
void Distance()
{
    // Check each enemy...
    for (int i = 0; i < targets.Length; i++){
            // ... to see how far they are from player
        float distance = Vector3.Distance(targets[i].transform.position, transform.position);
            // If social distancing is broken, then reduce health
        if (distance < distanceMinimum)
        {
            ... // Decrease health
        }
    }

}
```
<p align="center">
  <img src="Gifs/karensocialdistancing.gif" width="400" >
  <br>A Karen that infects the player on close range.
</p>

#### **Shooting Fireballs**

If a Karen is capable of shooting fireballs, it will do so when player is in sight.
```C#
// Shooting fireballs on sight - BossBehavior.cs

// If in sight...
if (d < distance && deltaHeight < 5) {
    CheckIfTimeToFire();    // Shoot fireballs
}
```

As mentioned, at a constant rate, fireballs will be taken from the object pool rather than being instanstiated. It will either explodes on collision with player, or vanish after a while in the air.
```C#
// Shoot fireball after a specified period of time - BossBehavior.cs
public void CheckIfTimeToFire()
{
    // When specified shooting time is reached
    if(Time.time > nextFire)
    {
        // Initiate a fireball from pool
        GameObject obj = ObjectPooler.SharedInstance.GetPooledObject("Fireball");
        if (obj != null) {
            obj.transform.position = transform.position;
            obj.transform.rotation = Quaternion.identity;
            obj.SetActive(true);

            // Set fireball to aim at player
            FireBallBehavior fb = obj.GetComponent<FireBallBehavior>();
            fb.Initiate();
        }

        nextFire = Time.time + fireRate; // Set timer for the next fire
    }
} 

// How a fireball can disappear - FireBallBehavior.cs
void Update () {
    // Fireball has limited lifetime
    if (Time.time - startTime > existTime) gameObject.SetActive(false);
}
void OnTriggerEnter(Collider other) {
    // If collides with player...
    if (other.gameObject.CompareTag("Player")) ... ; // Explode and decrease health
}
```

<p align="center">
  <img src="Gifs/fireballshoot.gif" width="400" >
  <br>A Karen continuously shooting fireballs at player.
</p>

### Level Switching & Vaccine

In each level, there will be 2 distinguishable stages: in-game and countdown. During the gameplay, the system will constantly check how many Karens are active in the map. 

When there is no Karens left, countdown will start and only in this period, the Holy Vaccine will be available for collect. 

New level is generated when countdown finishes.
```C#
// Implementation between levels - Player.cs

// In case of just finished a level...
if (targets.Length==0 && !isCountDown) {
	
	...// Set vaccine to be available

	// Start count down until next level
	startTime = Time.time;
	isCountDown = true;
	timeLeft = levelRelaxTime + startTime - Time.time;
} 

// In case of being in count down: update time left
else if (isCountDown) {
	timeLeft = levelRelaxTime + startTime - Time.time;
	
	// If time's up
	if (timeLeft < 0) {
		
		...// Holy Vaccine disappears
		
		// Generate new level
		level++;
		isCountDown = false;
		SpawnNewLevel();
		targets = GameObject.FindObjectsOfType<EnemyBehavior>();
	}
}
```

<p align="center">
  <img src="Gifs/levelchange.gif" width="400" >
  <br>Holy Vaccine appears during a 15-second interval between levels.
</p>

****

## Graphics and Camera

## Shaders and Particles

```
Descriptions of how the shaders work must be clearly detailed in the
report. It should be made clear how the use of a shader provides a
benefit over an equivalent CPU based approach, if applicable.
```

Evaluate on this very carefully guys! They mostly care abt this and evaluation!
* How shader works (and show which variables does what)
* How it is efficient to CPU
### Toon Shader

Toon shading which has another name is Cel shading is a rendering style designed to make 3D surfaces emulate 2D, flat surfaces. By using this shader, the objects will have the cartoon look as the name.

Toon shader contain 3 main parts. Firstly, it will receive lights from multiple light sources which reflects the real life lights in supermarket. Secondly, it will have specular reflection and then the rim lighting.
https://www.ronja-tutorials.com/2018/10/20/single-step-toon.html

https://roystan.net/articles/toon-shader.html
#### 1. Multiple Light Sources:
#### 2. Ambient Light:
#### 3. Specular Reflection:
#### 4. Rim Lighting :

### Outline Shader

<p align="center">
  <img src="Gifs/outline.gif" width="400" >
  <br>Items in the store being distinguished with outlines, instead of color reflections.
</p>

https://roystan.net/articles/outline-shader.html
### Half-tone Shader

<p align="center">
  <img src="Gifs/halftone.gif" width="400" >
  <br>A Karen being flashed. The light intensity is shown through circle density, rather than shades of color.
</p>

### Transparency Modification Shaders
The following Shaders are created based on:
* [A Unity tutorial on transparency](https://learn.unity.com/tutorial/writing-your-first-shader-in-unity)
* [A question thread on modifying transparency](https://answers.unity.com/questions/617420/change-transparency-of-a-shader.html)

**Foggy Shader**
Fog Shader uses position and pre-defined mask to modify the alpha channel of texture color, creating a varied opacity that resembles both a foggy and cyclonic effect. 

Even though simple, this shader is used for object types that takes the most number of occurrences in the game - Karens and collectibles. See GIF images on [Karen Control section](#karen-control) to see the effects being implemented on Karens.
```c#
float _Distance;
    sampler2D _Mask;    // Pre-made Mask to map opacity to object
    float _Speed;       // Offset speed
    fixed _ScrollDirX;  // Directions will affect the flow direction
    fixed _ScrollDirY;
    fixed4 _Color;      // Customize fog color if desired

    fixed4 frag(v2f i) : SV_Target
    {
        // Modifying UV coordinates to make texture rotate by time
        // For equivalent method in C#, see OffsetByTime.cs
        float2 uv = i.uv + fixed2(_ScrollDirX, _ScrollDirY) * _Speed * _Time.x;
        fixed4 col = tex2D(_MainTex, uv) * _Color * i.vertCol;
        // Modify opacity across positions using Mask to match alpha colors
        col.a *= tex2D(_Mask, i.uv2).r;
        col.a *= 1 - ((i.pos.z / i.pos.w) * _Distance);
        return col;
    }
```

<p align="center">
  <img src="Gifs/toilet.gif" width="400" >
  <br>The effect of a blue storm surrounding collectibles created by Fog Shader.
</p>

**Blinking Shader**
By modifying the alpha channel of the texture's color by time, blinking effect is created. This effect is used for the Holy Vaccine's container, which can be seen from the GIF image in [Level Switching & Vaccine section](#level-switching--vaccine).
```C#
float4 frag(vertOut input) : COLOR
    {

      float4 color = tex2D(_MainTex, float2(input.tex.xy));   
      
      if(color.a < _CutOff) discard;    // Limit the transparency if it is set
      else color.a = abs(sin(_Time.y)); // Varying opacity between (0,1) by time
      
      return color;
    }
```

### Particles

<p align="center">
  <img src="Gifs/bossfight3.gif" width="400" >
  <br>Particle effects shown in a boss fight.
</p>

#### Explosion

<p align="center">
  <img src="Gifs/explode.gif" width="400" >
  <br>A Karen being exploded.
</p>

#### Additive Blending

<p align="center">
  <img src="Gifs/bossfight.gif" width="400" >
  <br>A Boss Karen, where its overlaying particles will create a brighter particle.
</p>

## Evaluation Techniques

**Description of Method**

As part of the development of this game, two evaluation techniques were utilized to gather feedback from five external participants and improve the game. One querying method, 'cooperative evaluation', and one observational method, 'questionnaire', made up these two techniques.

We felt that these two techniques were very synergistic, since cooperative evaluation involves an ongoing dialogue during gameplay, effectively capturing player thoughts during a playthrough, and a questionnaire is completed after gameplay, after the player has had ample chance to reflect. This meant that we would gather useful insights at all stages of the player experience. Both were also practical given the current climate, since both had zero requirements for face-to-face contact.

For cooperative evaluation, the test user entered a 1-on-1 Zoom call with a member of the team, and would share their screen. During gameplay, the test user was invited to share any thoughts they had when playing the game, with emphasis on zero judgement for any comments made, which encouraged an open dialogue between the team member and test user. When the user was silent, the user was left to their own devices. During this session, the team member would take notes during the discussion on a Google Doc.

For the questionnaire, the test user was given a link to an online questionnaire roughly 10-15 minutes after completion of the game. See below for a link to the questionnaire:

https://www.surveymonkey.com/r/2ZJDMKM

The intent of this questionnaire was to uncover any core gameplay issues that users felt detracted from the quality of the game, and also prompted for any new features the user would like to see.

**Description of Participants**

For cooperative evaluation, a total of five different users were tested. All of these test users fell under a category of 'males aged 18-24 who frequently engage in games of this nature'. Whilst this set of test users is not diverse, we felt that their represented the core target market that such a game would have, and therefore the set of users whose comments and suggestions would be of greatest importance. Also, since these test users are all well-versed in the FPS genre, they possessed superior understanding of what makes an FPS game good compared to others, which allowed them to provide more specific and insightful feedback. 

For the questionnaire, a total of eight different users were tested, all different from those who did the cooperative evaluation. This group of users were more diverse, including two female users, but again all of these users fell under the desired '18-24 year old gamer' category, which represented our core audience. 

**Feedback**

The evaluation process outlined above gave rise to a plethora of feedback, some positive and most negative, that provided us with a clear roadmap for the last couple of days of development to produce a finished, polished product. The core positive was that the game was fundamentally 'fun', during cooperative evaluation, all five users engaged emotionally with the game and reacted in expected ways, for example being afraid of the Karen's, and laughing at the references prevalent throughout. However, there were a variety of criticisms that detracted from this fun, which included:

- A lack of sound cues for different activities, such as collecting loot or firing a gun
- FPS issues on less powerful machines, due to there being too many redundant assets in the game
- A narrow field of view, which gave the players an impression of being 'zoomed in'
- Initial disorientation at the start of the game, due to a poor spawning location
- Extremely high mouse sensitivity, which led to loss of control and confusion, this was the largest issue identified by all five to be game-breaking
- A lack of clarity in game instructions, which meant early on that players were unsure of what to do
- A difficulty of understanding when enemies were being hit and taking damage, due to a lack of a visible enemy health bar

These represented the issues deemed of 'critical' importance, out of three categories we created for feedback which included 'good to have' and 'less important'. Given the restricted timeframe for developing the game, we decided to categorize suggestions by importance, as well as difficulty of implementation. For example, almost all of the suggestions above were given an 'easy' difficulty, meaning that they were very simple fixes. It was decided  to action on anything within the 'critical' category, and anything 'easy or medium' in the 'good to have category', which we felt maximized quality of life within the game whilst keeping scope realistic. 

Some 'good to have' features that were implemented based on feedback included:

- Fireballs being shot from Karen's mouths instead of their feet, to better simulate coughing
- A slower rate of gunshot fire, so the user wouldn't have to spam click, an option for holding down left-click to fire, and ammunition mechanics
- A timer during rounds to kill all the Karen's, so the user would feel time pressure during rounds

And some 'less important' features that were left out included:

- Introducing a 2nd unique weapon, plus a scope mode for each weapon
- A mini-map that shows collectibles and enemy Karens
- Shooting and enemy movement animations

Overall, we found that the 'cooperative evaluation' part of our evaluation process to be of substantially greater value than the questionnaire. The reasoning for this was that 'think aloud' more readily enabled test users to go in-depth with their considerations of the game, since it was a conversational tone where team members could prompt for more depth. What we found with the questionnaire is that there was a lack of detail in responses, even when prompted, which we attributed to the lack of direct interaction with the team and thus there was less incentive to engage with the project. Being said, the questionnaire did produce some useful suggestions for improvement, but critical core gameplay issues such as FPS and mouse sensitivity were mostly ignored.

## Team Contributions

**Angus Hudson**

- Full development of gun, with firing particle and sound effects, ammunition mechanics
- Development of evaluation procedure, and write-up of evaluation feedback
- Refinement of first-person camera and control

