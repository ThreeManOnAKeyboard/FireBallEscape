using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesController : MonoBehaviour
{
	// The unique reference for this class
	public static AbilitiesController Instance;

	// Abilities array
	public Ability[] abilities;

	// UI references
	public Image abilityIconHolder;
	public Image abilityIconBackground;

	private Enumerations.DropType[] currentCombination = new Enumerations.DropType[3];
	private Ability currentAbility;

	private void Awake()
	{
		Instance = this;
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
	}

	public void OnAbilityClick()
	{
		// Check if ability is ready
		if (currentAbility != null)
		{
			// Trigger ability
			try
			{
				currentAbility.TriggerAbility();

				// Clean current ability
				currentAbility = null;
			}
			catch (System.Exception)
			{
				StartCoroutine(TriggerRefuse());
			}
		}
	}

	public void UpdateCombination(Enumerations.DropType dropType)
	{
		// Return if current ability holder is occupied
		if (currentAbility != null) return;

		// Update current combination
		for (int i = 0; i < currentCombination.Length; i++)
		{
			// Find an empty slot for new drop type holder
			if (currentCombination[i] == Enumerations.DropType.Empty)
			{
				currentCombination[i] = dropType;
			}
		}

		// If combination is fulfilled, then update the ability button
		if (currentCombination[currentCombination.Length - 1] != Enumerations.DropType.Empty)
		{
			UpdateCurrentAbility();
		}
	}

	private void UpdateCurrentAbility()
	{
		// Find ability by combination
		foreach (Ability ability in abilities)
		{
			if (ability.combination == currentCombination)
			{
				currentAbility = ability;
			}
		}

		// If ability is set then perform next changes
		if (currentAbility != null)
		{
			// Update UI
			abilityIconHolder.sprite = currentAbility.icon;
		}
	}

	// This is just for development stage
	private IEnumerator TriggerRefuse()
	{
		Text refuseText = GameObject.Find("RefuseText").GetComponent<Text>();
		refuseText.enabled = true;

		yield return new WaitForSeconds(2f);

		refuseText.enabled = false;
	}
}
