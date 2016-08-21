using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public static float health = 10;
	public float startHealth;
	public static float maximumHealth;
	public float maxHealth;

	[Range(0, 1)]
	public float minDamagePercent = 0;
	[Range(0, 1)]
	public float maxDamagePercent = 0;
	[Range(0, 1)]
	public float minHealPercent = 0;
	[Range(0, 1)]
	public float maxHealPercent = 0;

	public float healthDrainSpeed;
	public float healthBarFillSpeed;
	public Image healthBar;

	// The amount of time player is invincible
	public float invincibleDuration;

	// When to throw max power wave to blow things up before invicibility ending
	public float maxPowerWaveDelay;

	// The maximum speed when player is invincible
	public float invincibleSpeed;

	// Shield
	public GameObject shield;
	public float shieldDuration;

	// Some UI gameobjects
	public GameObject gameUI;
	public GameObject gameOverScreen;

	// Particle systems
	public GameObject defaultPE;
	public GameObject maxPowerPE;
	public GameObject maxPowerWave;

	// Instantiated after wave picking the wave drop
	public GameObject waveBlast;

	public static bool isInvincible;

	public Text scoreText;
	public static float score;
	private float previousY;

	void Awake()
	{
		health = startHealth;
		maximumHealth = maxHealth;
	}

	// Use this for initialization
	void Start()
	{
		healthBar.fillAmount = health / maxHealth;
	}

	// Update is called once per frame
	void Update()
	{
		if (!isInvincible)
		{
			health -= healthDrainSpeed * Time.deltaTime;
		}
		else
		{
			transform.position = new Vector3
			(
				Mathf.Lerp(transform.position.x, 0, invincibleSpeed * Time.deltaTime),
				transform.position.y + invincibleSpeed * Time.deltaTime,
				transform.position.z
			);
		}

		healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, healthBarFillSpeed * Time.deltaTime);

		if (health < 0)
		{
			gameOverScreen.SetActive(true);
			gameUI.SetActive(false);
			GameManager.Instance.ProcessScore();
			Destroy(gameObject);
		}
		else if (health > maxHealth)
		{
			isInvincible = true;
			defaultPE.SetActive(false);
			maxPowerPE.SetActive(true);

			// Reset all things to default after end of max power profit
			StartCoroutine(ResetToDefault());
		}

		// Update the score
		score += transform.position.y - previousY;
		scoreText.text = ((int)score).ToString();
		previousY = transform.position.y;
	}

	IEnumerator ResetToDefault()
	{
		health = maxHealth / 2f;
		yield return new WaitForSeconds(maxPowerWaveDelay);

		// Instantiate max power wave
		GameObject mpw = Instantiate(maxPowerWave);
		mpw.transform.position = transform.position;

		yield return new WaitForSeconds(invincibleDuration - maxPowerWaveDelay);

		isInvincible = false;
		defaultPE.SetActive(true);
		maxPowerPE.SetActive(false);
	}

	IEnumerator DeactivateShield()
	{
		yield return new WaitForSeconds(shieldDuration - 1f);

		// CHANGE THIS SHIT, IT'S JUST FOR TEST PURPOSE

		for (int i = 0; i < 5; i++)
		{
			shield.GetComponent<SpriteRenderer>().enabled = false;
			yield return new WaitForSeconds(0.1f);

			shield.GetComponent<SpriteRenderer>().enabled = true;
			yield return new WaitForSeconds(0.1f);
		}

		shield.SetActive(false);
	}

	public void ActivateShield()
	{
		if (shield.activeInHierarchy)
		{
			StopCoroutine("DeactivateShield");
		}
		else
		{
			shield.SetActive(true);
		}

		StartCoroutine("DeactivateShield");
	}

	public void ActicateWave()
	{
		// Instantiate max power wave
		GameObject mpw = Instantiate(waveBlast);
		mpw.transform.position = transform.position;
	}

	public void Damage()
	{
		// health = 5
		// maxHealth = 10
		// minDamagePercent = 0.05
		// maxDamagePercent = 0.25
		//						0.05 < 0.125 < 0.25
		// health = 5 - (10 * Clamp(0.5 * 0.25, 0.05, 0.25)) = 5 - (10 * 0.125) = 4.75
		if (!isInvincible)
		{
			health -= maxHealth * Mathf.Clamp((health / maxHealth) * maxDamagePercent, minDamagePercent, maxDamagePercent);
		}
	}

	public void Heal()
	{
		if (!isInvincible)
		{
			health += maxHealth * Mathf.Clamp((1 - health / maxHealth) * maxHealPercent, minHealPercent, maxHealPercent);
		}
	}

	public void EnableApplyRootMotion()
	{
		GetComponent<Animator>().applyRootMotion = true;
	}

	public void EnableGameUI()
	{
		gameUI.SetActive(true);
		score = 0;
	}

	public void EnableControlComponent()
	{
		switch (GameManager.Instance.controlType)
		{
			case GameManager.ControlType.FREE:
				GetComponent<TouchFollowMovement>().enabled = true;
				break;
			case GameManager.ControlType.SIDEWAYS:
				GetComponent<LeftRightMovement>().enabled = true;
				break;
			case GameManager.ControlType.ZIGZAG:
				GetComponent<ZigZagMovement>().enabled = true;
				break;
		}
	}
}
