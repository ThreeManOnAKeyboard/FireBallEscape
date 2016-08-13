using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
	private float health = 5;

	public float minHealth;
	public float maxHealth;

	public Image healthBar;
	public float healthBarFillSpeed;
	public float healthDrainSpeed;

	public GameObject gameUI;
	public GameObject gameOverScreen;

	// Use this for initialization
	void Start()
	{
		//healthBar.fillAmount = health / maxHealth;
	}

	// Update is called once per frame
	void Update()
	{
		//health -= healthDrainSpeed * Time.deltaTime;

		//healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, healthBarFillSpeed * Time.deltaTime);
	}

	public void AlterHealth(float amount)
	{
		health = Mathf.Clamp(health + amount, -1f, maxHealth);

		if (health < 0)
		{
			gameOverScreen.SetActive(true);
			gameUI.SetActive(false);
			Destroy(gameObject);
		}


		// Mathf.Clamp(currentHealth / maxHealth  maxPercent , minPercent, maxPercent)  maxHealth;
		//float maxHealth = 0;
		//float maxPercent = 0;
		//float minPercent = 0;

		//health += (int)(maxHealth * Mathf.Clamp((1 - health / maxHealth) * maxPercent, minPercent, maxPercent));

		//health -= (int)(maxHealth * Mathf.Clamp((health / maxHealth) * maxPercent, minPercent, maxPercent));

		//health -= drainSpeed * Time.deltaTime;
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
