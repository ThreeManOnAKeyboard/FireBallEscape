using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
	public float healthBarFillSpeed;
	private Image healthBar;

	void Awake()
	{
		healthBar = GetComponent<Image>();
	}

	// Use this for initialization
	void Start()
	{
		healthBar.fillAmount = PlayerController.health / PlayerController.maximumHealth;
	}

	// Update is called once per frame
	void Update()
	{
		healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, PlayerController.health / PlayerController.maximumHealth, healthBarFillSpeed * Time.deltaTime);
	}
}
