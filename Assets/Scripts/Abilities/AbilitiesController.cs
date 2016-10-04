using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesController : MonoBehaviour
{
	public Ability[] abilities;
	public SuperAbility[] superAbilities;
	public Image abilityIconHolder;
	public Image abilityIconBackground;
	public float backgroundFillSpeed;

	private float currentFuelAmount;
	private float currentBackgroundFillAmount;
	private int currentAbilityIndex;

	// Super Abilities related stuff
	private SuperAbility.Combination[] currentCombination;

	private void Awake()
	{
		int maxComboLength = superAbilities[0].combination.Length;

		for (int i = 0; i < superAbilities.Length; i++)
		{
			if (superAbilities[i].combination.Length > maxComboLength)
			{
				maxComboLength = superAbilities[i].combination.Length;
			}
		}

		currentCombination = new SuperAbility.Combination[maxComboLength];
	}

	// Update is called once per frame
	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetButtonDown("Jump"))
		{
			OnAbilityClick();
		}
#endif

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

		try
		{
			abilities[currentAbilityIndex].TriggerAbility();
			currentAbilityIndex = 0;
			ChangeCurrentAbility();
		}
		catch (System.Exception)
		{
			//StartCoroutine(TriggerRefuse());
		}
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
				CheckCombination(isFuelDrop);
			}
		}
		else
		{
			CheckCombination(isFuelDrop);
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

	// This is just for development stage
	private IEnumerator TriggerRefuse()
	{
		Text refuseText = GameObject.Find("RefuseText").GetComponent<Text>();
		refuseText.enabled = true;

		yield return new WaitForSeconds(2f);

		refuseText.enabled = false;
	}

	private void CheckCombination(bool isFuelDrop)
	{
		// Move combination
		for (int i = 0; i < currentCombination.Length - 1; i++)
		{
			currentCombination[i] = currentCombination[i + 1];
		}

		// Add the last element of combination
		currentCombination[currentCombination.Length - 1] = isFuelDrop ? SuperAbility.Combination.Fuel : SuperAbility.Combination.Water;

		// Check if any super ability has to be triggered
		for (int i = 0; i < superAbilities.Length; i++)
		{
			for (int j = 0; j <= currentAbilityIndex; j++)
			{
				// Check if super ability is available
				if (superAbilities[i].requiredAbility == abilities[j])
				{
					// Check the combination
					SuperAbility.Combination[] comboToCheck = superAbilities[i].combination;
					int k;
					for (k = 1; k <= comboToCheck.Length; k++)
					{
						if (comboToCheck[comboToCheck.Length - k] != currentCombination[currentCombination.Length - k])
						{
							break;
						}
					}

					if (k == comboToCheck.Length + 1)
					{
						// Trigger Super Ability
						superAbilities[i].TriggerAbility();
						currentCombination = new SuperAbility.Combination[currentCombination.Length];
						break;
					}
				}
			}
		}
	}
}
