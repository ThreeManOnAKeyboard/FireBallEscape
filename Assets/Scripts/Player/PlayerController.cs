using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public static float health = 10f;
	public static float maximumHealth;

	[Header("Health Parameters")]
	public float startHealth;
	public float maxHealth;

	[Range(0f, 1f)]
	public float minDamagePercent = 0f;
	[Range(0f, 1f)]
	public float maxDamagePercent = 0f;
	[Range(0f, 1f)]
	public float minHealPercent = 0f;
	[Range(0f, 1f)]
	public float maxHealPercent = 0f;

	[Header("Game UI References")]
	public GameObject gameUI;
	public GameObject gameOverScreen;

	[Header("Death Explosion Prefab")]
	public GameObject deathExplosion;

	public static bool isConstHealth;
	public static float targetHealth;

	private ParticleSystem.ShapeModule thisShape;

	void Awake()
	{
		thisShape = GetComponentInChildren<ParticleSystem>().shape;
		health = startHealth;
		maximumHealth = maxHealth;
		targetHealth = maximumHealth;
		isConstHealth = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (health <= 0f)
		{
			Time.timeScale = 1f;
			Time.fixedDeltaTime = 0.02f;
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
	public void Damage(float multiplier)
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

		if (!isConstHealth)
		{
			health -= multiplier * maxHealth * Mathf.Clamp((health / maxHealth) * maxDamagePercent, minDamagePercent, maxDamagePercent);
		}
	}

	public void Heal(float multiplier)
	{
		if (!isConstHealth)
		{
			if (targetHealth != maximumHealth)
			{
				targetHealth = maximumHealth;
			}
			else
			{
				health += multiplier * maxHealth * Mathf.Clamp((1 - health / maxHealth) * maxHealPercent, minHealPercent, maxHealPercent);
			}
		}
	}

	public void FullHeal()
	{
		if (!isConstHealth)
		{
			targetHealth = maximumHealth;
			health = maximumHealth;
		}
	}

	public void Kill()
	{
		if (!isConstHealth)
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
			case Enumerations.ControlType.Free:
				GetComponent<TouchFollowMovement>().enabled = true;
				break;
			case Enumerations.ControlType.Sideways:
				GetComponent<LeftRightMovement>().enabled = true;
				break;
			case Enumerations.ControlType.ZigZag:
				GetComponent<ZigZagMovement>().enabled = true;
				break;
		}
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
		while (health > targetHealth && !isConstHealth)
		{
			health -= drainSpeed * Time.deltaTime;

			yield return null;
		}

		targetHealth = maximumHealth;
	}
	#endregion

	#region Stone Collision Effect
	public void OnStoneCollisionEffect(float effectDuration)
	{
		if (thisShape.shapeType != ParticleSystemShapeType.Cone)
		{
			StartCoroutine(DoStoneCollisionEffect(effectDuration));
		}
	}

	private IEnumerator DoStoneCollisionEffect(float effectDuration)
	{
		thisShape.shapeType = ParticleSystemShapeType.ConeVolume;

		yield return new WaitForSeconds(effectDuration);

		thisShape.shapeType = ParticleSystemShapeType.Sphere;
	}
	#endregion
}
