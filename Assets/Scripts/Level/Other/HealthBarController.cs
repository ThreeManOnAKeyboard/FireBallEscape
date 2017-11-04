using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Level.Other
{
	public class HealthBarController : MonoBehaviour
	{
		public float healthBarFillSpeed;
		public float poisonBarFillSpeed;
		public Image poisonBar;
		public Image sparkle;
		private Image healthBar;

		private void Awake()
		{
			healthBar = GetComponent<Image>();
		}

		// Use this for initialization
		private void Start()
		{
			healthBar.fillAmount = PlayerController.health / PlayerController.maximumHealth;
			poisonBar.fillAmount = 1f - PlayerController.targetHealth / PlayerController.maximumHealth;
		}

		// Update is called once per frame
		private void Update()
		{
			// Update the health bar UI
			healthBar.fillAmount = Mathf.Lerp
			(
				healthBar.fillAmount,
				PlayerController.health / PlayerController.maximumHealth,
				healthBarFillSpeed * Time.unscaledDeltaTime
			);

			// Update the poison bar UI
			poisonBar.fillAmount = Mathf.Lerp
			(
				poisonBar.fillAmount,
				1f - PlayerController.targetHealth / PlayerController.maximumHealth,
				poisonBarFillSpeed * Time.unscaledDeltaTime
			);

			//Update the Sparkle Image position
			sparkle.rectTransform.anchoredPosition = new Vector3(healthBar.rectTransform.sizeDelta.x * (healthBar.fillAmount - 0.01f), 20, 0);

		}
	}
}
