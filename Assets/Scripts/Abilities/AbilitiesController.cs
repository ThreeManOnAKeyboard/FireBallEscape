using UnityEngine;
using UnityEngine.UI;

public class AbilitiesController : MonoBehaviour
{
	public Ability[] abilities;
	public Image abilityIconHolder;
	public Image abilityIconBackground;
	public float backgroundFillSpeed;

	private float currentFuelAmount;
	private float currentBackgroundFillAmount;
	private int currentAbilityIndex;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// Smoothly update the ability icon background fill amount
		abilityIconBackground.fillAmount = Mathf.Lerp
		(
			abilityIconBackground.fillAmount,
			currentBackgroundFillAmount,
			Time.deltaTime * backgroundFillSpeed
		);
	}

	public void OnAbilityClick()
	{
		// Check if at least first ability is available
		if (currentAbilityIndex == 0)
		{
			return;
		}

		abilities[currentAbilityIndex].TriggerAbility();
		currentAbilityIndex = 0;
		ChangeCurrentAbility();
	}

	public void UpdateAbility(bool isFuelDrop)
	{
		// If there are more available abilities then check the fuel amount
		if (currentAbilityIndex < (abilities.Length - 1))
		{
			// Add / Substract fuel amount by 1
			currentFuelAmount = Mathf.Clamp(currentFuelAmount + (isFuelDrop ? 1f : -1f), 0f, abilities[currentAbilityIndex + 1].neededFuel);

			if (currentFuelAmount == abilities[currentAbilityIndex + 1].neededFuel)
			{
				// Update ability
				currentAbilityIndex++;
				ChangeCurrentAbility();
			}
			else
			{
				// Update fill amount
				currentBackgroundFillAmount = currentFuelAmount / abilities[currentAbilityIndex + 1].neededFuel;
			}
		}
	}

	private void ChangeCurrentAbility()
	{
		// Reset things
		currentFuelAmount = 0f;
		currentBackgroundFillAmount = 0f;

		// Update things
		abilityIconHolder.sprite = abilities[currentAbilityIndex].abilityIcon;
	}
}
