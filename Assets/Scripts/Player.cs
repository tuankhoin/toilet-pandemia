﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	public int maxHealth = 1000;
	public int currentHealth;
	public int score = 0;
	public int damage = -1;
	public double distanceMinimum = 1.5;
	public HealthBar healthBar;
	public Pause pause;

	public GameObject[] targets;
    // Start is called before the first frame update
    void Start()
    {
		Debug.Log(Global.inGame);
		if (Global.inGame) {
			currentHealth = Global.currentHealth;
			score = Global.overallScore;
		}
		else {
			currentHealth = maxHealth;
		}

		healthBar.SetMaxHealth(maxHealth);
		healthBar.SetHealth(currentHealth);

		targets = GameObject.FindGameObjectsWithTag("Enemy");
		Global.inGame = true;
	}

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.F1))
		{
			ChangeHealth(+200);
		}
		if (!pause.isPaused){
			Distance();
		}
		targets = GameObject.FindGameObjectsWithTag("Enemy");

		if (Input.GetKeyDown(KeyCode.F2))
		{
			SceneManager.LoadScene(2 + (int) Mathf.Cos(-Mathf.PI*SceneManager.GetActiveScene().buildIndex/2));
		}

		Global.overallScore = score;
    }

	void OnLevelWasLoaded(int thisLevel)
	{
		transform.position = GameObject.FindWithTag("Spawn").transform.position;
	}

	void OnTriggerEnter(Collider other)
	{		
		if (other.gameObject.CompareTag("Fireball"))
		{
			ChangeHealth(-150);
		}
	}

		void Distance()
    {
		for (int i = 0; i < targets.Length; i++){
			float distance = Vector3.Distance(targets[i].transform.position, transform.position);
			if (distance < distanceMinimum)
			{
				ChangeHealth(damage);
			}
		}
		
	}

	public void ChangeHealth(int delta)
	{
		currentHealth += delta;
		if (currentHealth > maxHealth) {
			currentHealth = maxHealth;
		}
		if (currentHealth < 0)
        {
			if (Global.overallScore > Global.maxScore) {
				Global.maxScore = Global.overallScore;
				PlayerPrefs.SetInt("highscore", score);
			}

			Global.inGame = false;
			SceneManager.LoadScene(3);
        }

		healthBar.SetHealth(currentHealth);
		Global.currentHealth = currentHealth;
	}
}

public static class Global {
	public static int overallScore;
	public static int maxScore = PlayerPrefs.GetInt("highscore", 0);
	public static bool inGame = false;
	public static int currentHealth;
}
