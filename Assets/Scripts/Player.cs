using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	public int maxHealth = 1000;
	public int currentHealth;
	public int score = 0;
	public int level = 1;
	public int damage = -1;
	public double distanceMinimum = 1.5;
	public HealthBar healthBar;
	public Pause pause;
	public DamageOverlay damageOverlay;

	public GameObject[] targets;
    // Start is called before the first frame update
    void Start()
    {
		if (Global.inGame) {
			currentHealth = Global.currentHealth;
			score = Global.overallScore;
		}
		else {
			Global.currentHealth = maxHealth;
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
		targets = GameObject.FindGameObjectsWithTag("Enemy");

		if (Input.GetKeyDown(KeyCode.F1))
		{
			ChangeHealth(+200);
		}
		if (!pause.isPaused){
			Distance();
		}

		if (Input.GetKeyDown(KeyCode.F2))
		{
			SceneManager.LoadScene(2 + (int) Mathf.Cos(-Mathf.PI*SceneManager.GetActiveScene().buildIndex/2));
		}
	}

	void OnTriggerEnter(Collider other)
	{		
		if (other.gameObject.CompareTag("Fireball"))
		{
			ChangeHealth(-150);
			damageOverlay.SetDamage(true);
		}
	}

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

			// Global.currentHealth = maxHealth;
			Global.inGame = false;
			SceneManager.LoadScene(3);
			return;
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
