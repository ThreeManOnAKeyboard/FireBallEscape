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

	// The maximum speed when player is invincible
	public float invincibleSpeed;

	// Some UI gameobjects
	public GameObject gameUI;
	public GameObject gameOverScreen;

	// Particle systems
	public GameObject defaultParticleSystem;
	public GameObject maxPowerParticleSystem;

	public static bool isInvincible;

	public Text scoreText;
	private float scoreMultiplier = 1;
	private float score;
	private Vector3 previousPosition;

	// Reference to Ability Controller
	private AbilitiesController abilitiesController;

	void Awake()
	{
		health = startHealth;
		maximumHealth = maxHealth;
		isInvincible = false;
	}

	// Use this for initialization
	void Start()
	{
		// Find the AbilitiesController Component
		abilitiesController = FindObjectOfType<AbilitiesController>();
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

		if (health < 0)
		{
			gameOverScreen.SetActive(true);
			gameUI.SetActive(false);
			GameManager.Instance.ProcessScore();
			Destroy(gameObject);
		}

		// Update the score
		score += (transform.position.y - previousPosition.y) * scoreMultiplier;
		scoreText.text = ((int)score).ToString();

		// Save previous position for next frame calculations
		previousPosition = transform.position;
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
			abilitiesController.UpdateAbility(false);
		}
	}

	public void Heal()
	{
		if (!isInvincible)
		{
			health += maxHealth * Mathf.Clamp((1 - health / maxHealth) * maxHealPercent, minHealPercent, maxHealPercent);
			abilitiesController.UpdateAbility(true);
		}
	}

	public void Kill()
	{
		if (!isInvincible)
		{
			health = 0;
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

	public void AddScore(float scoreAmount)
	{
		score += scoreAmount * scoreMultiplier++;
	}

	public void ResetMultiplier()
	{
		if (!isInvincible)
		{
			scoreMultiplier = 1;
		}
	}

	public float GetScore()
	{
		return score;
	}
}
