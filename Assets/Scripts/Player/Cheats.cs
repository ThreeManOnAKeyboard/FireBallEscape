using UnityEngine;
using UnityEngine.UI;

public class Cheats : MonoBehaviour
{
#if UNITY_EDITOR
	public KeyCode healKeyCode;
	public KeyCode shakeKeyCode;
	public KeyCode toggleInvincabilityKeyCode;

	public Text healText;
	public Text shakeText;
	public Text invincibleText;

	public bool bifa;

	private PlayerController playerController;

	// Use this for initialization
	void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();

		if (bifa)
		{
			healText.text = "Press " + healKeyCode.ToString() + " to heal";
			shakeText.text = "Press " + shakeKeyCode.ToString() + " to shake camera";
			invincibleText.text = "Press " + toggleInvincabilityKeyCode.ToString() + " to enable invincible state";
		}
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKey(healKeyCode))
		{
			playerController.Heal(0.4f);
		}

		// Simulate camera shake
		if (Input.GetKeyDown(shakeKeyCode))
		{
			Camera.main.gameObject.GetComponent<CameraShake>().StartShake
			(
				Random.Range(0.5f, 2f),
				Random.Range(10f, 15f),
				Random.Range(0.2f, 0.5f),
				Random.Range(1f, 2f)
			);
		}

		if (Input.GetKeyDown(toggleInvincabilityKeyCode))
		{
			PlayerController.isInvincible = !PlayerController.isInvincible;

			if (bifa)
			{
				if (PlayerController.isInvincible)
				{
					invincibleText.text = "Press " + toggleInvincabilityKeyCode.ToString() + " to disable invincible state";
				}
				else
				{
					invincibleText.text = "Press " + toggleInvincabilityKeyCode.ToString() + " to enable invincible state";
				}
			}
		}
	}
#endif
}
