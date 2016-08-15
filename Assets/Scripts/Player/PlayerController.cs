using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	public static float health = 10;
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

	// The maximum speed when player is invincible
	public float invincibleSpeed;

	// Some UI gameobjects
	public GameObject gameUI;
	public GameObject gameOverScreen;

	// Particle systems
	public GameObject defaultPE;
	public GameObject maxPowerPE;

	public static bool isInvincible;

	void Awake()
	{
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
	}

	IEnumerator ResetToDefault()
	{
		yield return new WaitForSeconds(invincibleDuration);

		health = maxHealth / 2f;
		isInvincible = false;
		defaultPE.SetActive(true);
		maxPowerPE.SetActive(false);
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
