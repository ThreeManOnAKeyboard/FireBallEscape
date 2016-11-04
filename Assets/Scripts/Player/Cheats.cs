using UnityEngine;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
#if UNITY_EDITOR
	[Header("Key Codes")]
	public KeyCode heal;
	public KeyCode shake;
	public KeyCode toggleInvincability;
	public KeyCode fuelElement;
	public KeyCode waterElement;
	public KeyCode poisonElement;

	[Header("Help Texts")]
	public Text healText;
	public Text shakeText;
	public Text invincibleText;

	[Header("Turn off to hide tips")]
	public bool showTips;

	private PlayerController playerController;

	// Use this for initialization
	void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();

		if (showTips)
		{
			healText.text = "Press " + heal.ToString() + " to heal";
			shakeText.text = "Press " + shake.ToString() + " to shake camera";
			invincibleText.text = "Press " + toggleInvincability.ToString() + " to enable invincible state";
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(heal))
		{
			playerController.Heal(0.4f);
		}

		// Simulate camera shake
		if (Input.GetKeyDown(shake))
		{
			Camera.main.gameObject.GetComponent<CameraShake>().StartShake
			(
				Random.Range(0.5f, 2f),
				Random.Range(10f, 15f),
				Random.Range(0.2f, 0.5f),
				Random.Range(1f, 2f)
			);
		}

		if (Input.GetKeyDown(toggleInvincability))
		{
			PlayerController.isConstHealth = !PlayerController.isConstHealth;

			if (showTips)
			{
				if (PlayerController.isConstHealth)
				{
					invincibleText.text = "Press " + toggleInvincability.ToString() + " to disable invincible state";
				}
				else
				{
					invincibleText.text = "Press " + toggleInvincability.ToString() + " to enable invincible state";
				}
			}
		}

		if (Input.GetKeyDown(fuelElement))
		{
			AbilitiesController.Instance.UpdateCombination(Enumerations.DropType.Fuel);
		}

		if (Input.GetKeyDown(waterElement))
		{
			AbilitiesController.Instance.UpdateCombination(Enumerations.DropType.Water);
		}

		if (Input.GetKeyDown(poisonElement))
		{
			AbilitiesController.Instance.UpdateCombination(Enumerations.DropType.Poison);
		}
	}
#endif
}
