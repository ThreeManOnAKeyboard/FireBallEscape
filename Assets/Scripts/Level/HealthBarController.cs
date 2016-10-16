using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
	public float healthBarFillSpeed;
	public float poisonBarFillSpeed;
	public Image poisonBar;
	private Image healthBar;

	void Awake()
	{
		healthBar = GetComponent<Image>();
	}

	// Use this for initialization
	void Start()
	{
		healthBar.fillAmount = PlayerController.health / PlayerController.maximumHealth;
		poisonBar.fillAmount = 1f - PlayerController.targetHealth / PlayerController.maximumHealth;
	}

	// Update is called once per frame
	void Update()
	{
		// Update the health bar UI
		healthBar.fillAmount = Mathf.Lerp
		(
			healthBar.fillAmount,
			PlayerController.health / PlayerController.maximumHealth,
			healthBarFillSpeed * Time.deltaTime
		);

		// Update the poison bar UI
		poisonBar.fillAmount = Mathf.Lerp
		(
			poisonBar.fillAmount,
			1f - PlayerController.targetHealth / PlayerController.maximumHealth,
			poisonBarFillSpeed * Time.deltaTime
		);
	}
}
