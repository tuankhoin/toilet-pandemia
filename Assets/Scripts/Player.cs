using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
	public static Player SharedInstance;
	public int maxHealth = 1000;
	public int currentHealth;
	public int score = 0;
	public int level = 1;
	public int damage = -1;
	public int fireballDamage = -150;
	public double distanceMinimum = 1.5;
	public float levelRelaxTime = 15.0f;
	public HealthBar healthBar;
	public Pause pause;
	public DamageOverlay damageOverlay;

	public EnemyBehavior[] targets;
	public bool isCountDown = false;
	public float startTime;
	public float timeLeft = 0f;
	public AudioSource damageSFX;
	HolyVaccine vaxx;
	EnemyLock pole;

	void Awake () {
		SharedInstance = this;
	}

    // Start is called before the first frame update
    void Start()
    {
		Global.spawnLocations = GameObject.FindGameObjectsWithTag("Spawn");
		isCountDown = false;
		currentHealth = maxHealth;

		healthBar.SetMaxHealth(maxHealth);
		healthBar.SetHealth(currentHealth);

		targets = GameObject.FindObjectsOfType<EnemyBehavior>();
		vaxx = GameObject.FindObjectOfType<HolyVaccine>();
		pole = GameObject.FindObjectOfType<EnemyLock>();
		vaxx.gameObject.SetActive(false);
	}

    // Update is called once per frame
    void Update()
	{
		// For debugging on harder levels
		if (Input.GetKeyDown(KeyCode.F1))
		{
			ChangeHealth(+200);
		}

		// Only play the game if it is not paused, of course
		if (!pause.isPaused){
			Distance();
		}

		// In case of just finished a level...
		if (targets.Length==0 && !isCountDown) {
			
			// Show the Holy Vaccine
			vaxx.gameObject.SetActive(true);
			pole.gameObject.SetActive(false);
			
			// Set count down time until next level
			startTime = Time.time;
			isCountDown = true;
			timeLeft = levelRelaxTime + startTime - Time.time;
		} 
		
		// In case of being in count down: update time left
		else if (isCountDown) {
			timeLeft = levelRelaxTime + startTime - Time.time;
			
			// If time's up
			if (timeLeft < 0) {
				
				// Holy Vaccine disappears
				vaxx.gameObject.SetActive(false);
				pole.gameObject.SetActive(true);
				
				// Generate new level
				level++;
				isCountDown = false;
				SpawnNewLevel();
				targets = GameObject.FindObjectsOfType<EnemyBehavior>();
			}
		}
	}

	// Spawn bonuses and enemies from object pool when a new level is loaded
	void SpawnNewLevel() {

		// Look through each pool of item
		foreach (ObjectPoolItem item in ObjectPooler.SharedInstance.itemsToPool) {
			string tag = item.objectToPool.tag;

			// Fireball is only stored for boss karens only
			if (tag == "Fireball") continue;

			// Pick items with needed quantity (by level), and set to be active in map
            for (int i = 0; i < Mathf.FloorToInt(item.amountToPoolEachLevel * level); i++) {
                GameObject obj = ObjectPooler.SharedInstance.GetPooledObject(tag);
				if (obj != null) {
					obj.GetComponent<randomSpawn>().SetPosition();
                	obj.SetActive(true);
				}
            }
        }
	}

	// Take away health if player got hit by karen's fireball
	void OnTriggerEnter(Collider other)
	{		
		if (other.gameObject.CompareTag("Fireball"))
		{
			ChangeHealth(fireballDamage);
			damageOverlay.SetDamage(true);
			damageSFX.Play();
		}
	}

	// Check if social distancing is maintained. If not, drain health
	void Distance()
    {
		for (int i = 0; i < targets.Length; i++){
			float distance = Vector3.Distance(targets[i].transform.position, transform.position);
			if (distance < distanceMinimum)
			{
				ChangeHealth(damage);
				damageOverlay.SetDamage(true);
			}
		}
		
	}

	// Update health accordingly
	public void ChangeHealth(int delta)
	{
		currentHealth += delta;

		// If health is full, limit to max health
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}

		// If run out of health, game over
		else if (currentHealth < 0)
        {
			Cursor.lockState = CursorLockMode.None;

			// Check and set high score
			Global.overallScore = score;
			if (Global.overallScore > Global.maxScore) {
				Global.maxScore = Global.overallScore;
				PlayerPrefs.SetInt("highscore", score);
			}

			SceneManager.LoadScene("GameOver");
			return;
        }

		// Update health bar as well
		healthBar.SetHealth(currentHealth);
	}
}

// Global variables to store high score, as well as saving up computation
public static class Global {
	public static int overallScore;
	public static int maxScore = PlayerPrefs.GetInt("highscore", 0);
	public static GameObject [] spawnLocations;
}
