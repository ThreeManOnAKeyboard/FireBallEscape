using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public class Element
{
	public Enumerations.DropType dropType;
	public Color color;
}

public class AbilitiesController : MonoBehaviour
{
	#region Fields
	private const int ELEMENTS_COUNT = 3;

	[Header("Elements properties")]
	public Color defaultElementColor;   // Should be transparent
	public List<Element> elements = new List<Element>(ELEMENTS_COUNT);
	public Image[] elementsImages = new Image[ELEMENTS_COUNT];

	// The unique reference for this class
	public static AbilitiesController Instance;

	[Header("Ability properties")]
	public Sprite defaultAbilityImage;
	// Abilities array
	public List<Ability> abilities;

	// UI references
	public Image abilityIconHolder;
	public Image abilityIconBackground;

	// Ability references
	private Enumerations.DropType[] currentCombination = new Enumerations.DropType[ELEMENTS_COUNT];
	private Ability currentAbility;
	#endregion

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

	public void OnValidate()
	{
		if (elements.Count != ELEMENTS_COUNT)
		{
			Debug.LogWarning("Don't resize this list!!!");

			if (elements.Count > ELEMENTS_COUNT)
			{
				elements.RemoveRange(ELEMENTS_COUNT, elements.Count - ELEMENTS_COUNT);
			}
			else
			{
				Debug.LogError("Configure again the elements list and never resize it again");
			}
		}

		if (elementsImages.Length != ELEMENTS_COUNT)
		{
			Debug.LogWarning("Don't resize this array!!!");
			Array.Resize(ref elementsImages, ELEMENTS_COUNT);
		}
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
				abilityIconHolder.sprite = defaultAbilityImage;
			}
			catch (Exception)
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
			// Find an empty slot for new drop type insertion
			if (currentCombination[i] == Enumerations.DropType.Empty)
			{
				currentCombination[i] = dropType;
				elementsImages[i].color = elements.Find(element => element.dropType == dropType).color;

				break;
			}
		}

		// If combination is fulfilled, then update the ability button
		if (currentCombination[currentCombination.Length - 1] != Enumerations.DropType.Empty)
		{
			UpdateCurrentAbility();
			ResetElements();
		}
	}

	private void UpdateCurrentAbility()
	{
		// Find ability by combination
		currentAbility = abilities.Find(ability => CompareCombinations(ability.combination, currentCombination));
		currentCombination = new Enumerations.DropType[ELEMENTS_COUNT];

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

	// Reset elements to their default state
	private void ResetElements()
	{
		foreach (Image elementObject in elementsImages)
		{
			elementObject.color = defaultElementColor;
		}
	}

	private bool CompareCombinations(Enumerations.DropType[] combinationOne, Enumerations.DropType[] combinationTwo)
	{
		Enumerations.DropType[] combo1 = combinationOne.OrderBy(drop => (byte)drop).ToArray();
		Enumerations.DropType[] combo2 = combinationTwo.OrderBy(drop => (byte)drop).ToArray();

		for (int i = 0; i < ELEMENTS_COUNT; i++)
		{
			if (combo1[i] != combo2[i])
			{
				return false;
			}
		}

		return true;
	}
}
