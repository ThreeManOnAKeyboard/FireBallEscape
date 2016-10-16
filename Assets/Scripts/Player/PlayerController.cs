using System.Collections;
using UnityEngine;

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

	// Some UI gameobjects
	public GameObject gameUI;
	public GameObject gameOverScreen;

	// Particle systems
	public GameObject defaultParticleSystem;
	public GameObject maxPowerParticleSystem;
	public GameObject deathExplosion;

	public static bool isInvincible;
	public bool isUnderSuperShield;

	private Vector3 previousPosition;

	// The maximum speed when player is invincible
	private float invincibleSpeed;

	public static float targetHealth;

	// Reference to Ability Controller script
	private AbilitiesController abilitiesController;

	void Awake()
	{
		health = startHealth;
		maximumHealth = maxHealth;
		targetHealth = maximumHealth;
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
		if (isInvincible)
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
			ScoreManager.Instance.ProcessScore();
			Instantiate(deathExplosion).transform.position = transform.position;
			Destroy(gameObject);
		}
		else if (health > maxHealth)
		{
			health = maxHealth;
		}
	}

	#region Health related methods
	public void Damage(bool isDrop)
	{
		#region Calculus example
		////
		// health = 5
		// maxHealth = 10
		// minDamagePercent = 0.05
		// maxDamagePercent = 0.25
		//						0.05 < 0.125 < 0.25
		// health = 5 - (10 * Clamp(0.5 * 0.25, 0.05, 0.25)) = 5 - (10 * 0.125) = 4.75
		////
		#endregion

		if (!isInvincible)
		{
			health -= maxHealth * Mathf.Clamp((health / maxHealth) * maxDamagePercent, minDamagePercent, maxDamagePercent);

			if (isDrop)
			{
				abilitiesController.UpdateAbility(false);
			}
		}
	}

	public void Heal(bool isDrop)
	{
		if (!isInvincible)
		{
			if (targetHealth != maximumHealth)
			{
				targetHealth = maximumHealth;
			}
			else
			{
				health += maxHealth * Mathf.Clamp((1 - health / maxHealth) * maxHealPercent, minHealPercent, maxHealPercent);
			}

			if (isDrop)
			{
				abilitiesController.UpdateAbility(true);
			}
		}
	}

	public void Kill()
	{
		if (!isInvincible)
		{
			health = 0;
		}
	}
	#endregion

	#region On game start actions
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
	#endregion

	#region Ultimate effect
	public void ActivateUltimate(float speed)
	{
		isInvincible = true;
		defaultParticleSystem.SetActive(false);
		maxPowerParticleSystem.SetActive(true);
		invincibleSpeed = speed;
	}

	public void DeactivateUltimate()
	{
		isInvincible = false;
		defaultParticleSystem.SetActive(true);
		maxPowerParticleSystem.SetActive(false);
	}
	#endregion

	#region Poison drop effect
	public void StartHealthDrain(float amount, float drainSpeed)
	{
		if (targetHealth != maximumHealth)
		{
			targetHealth = targetHealth - amount;
		}
		else
		{
			targetHealth = health - amount;
			StartCoroutine(DrainHealth(drainSpeed));
		}

		if (targetHealth < 0f)
		{
			targetHealth = 0f;
		}
	}

	private IEnumerator DrainHealth(float drainSpeed)
	{
		while (health > targetHealth)
		{
			health -= drainSpeed * Time.deltaTime;

			yield return null;
		}

		targetHealth = maximumHealth;
	}
	#endregion
}
