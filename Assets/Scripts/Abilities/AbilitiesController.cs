using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using _3rdParty;

namespace Abilities
{
	public class AbilitiesController : MonoBehaviour
	{
		#region Fields
		private const int ElementsCount = 3;

		[Header("Elements properties")]
		public Sprite defaultElementImage;
		public List<Element> elements = new List<Element>(ElementsCount);
		public Image[] elementsImages = new Image[ElementsCount];

		// The unique reference for this class
		public static AbilitiesController instance;

		[Header("Ability properties")]
		public Sprite defaultAbilityImage;
		// Abilities array
		public List<Ability> abilities;

		// UI references
		public Image abilityIconHolder;

		// Ability references
		//private Enumerations.DropType[] currentCombination = new Enumerations.DropType[ElementsCount];
		private Ability currentAbility;
		#endregion

		private void Awake()
		{
			instance = this;
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
			if (elements.Count != ElementsCount)
			{
				Debug.LogWarning("Don't resize this list!!!");

				if (elements.Count > ElementsCount)
				{
					elements.RemoveRange(ElementsCount, elements.Count - ElementsCount);
				}
				else
				{
					Debug.LogError("Configure again the elements list and never resize it again");
				}
			}

			if (elementsImages.Length != ElementsCount)
			{
				Debug.LogWarning("Don't resize this array!!!");
				Array.Resize(ref elementsImages, ElementsCount);
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
					ResetElements();
				}
				catch (Exception)
				{
					StartCoroutine(TriggerRefuse());
				}
			}
		}

		public void UpdateCombination(Enumerations.DropType dropType)
		{
			//// Return if current ability holder is occupied
			//if (currentAbility != null) return;

			//// Update current combination
			//for (int i = 0; i < currentCombination.Length; i++)
			//{
			//	// Find an empty slot for new drop type insertion
			//	if (currentCombination[i] == Enumerations.DropType.Empty)
			//	{
			//		currentCombination[i] = dropType;
			//		elementsImages[i].sprite = elements.Find(element => element.dropType == dropType).image;

			//		break;
			//	}
			//}

			//// If combination is fulfilled, then update the ability button
			//if (currentCombination[currentCombination.Length - 1] != Enumerations.DropType.Empty)
			//{
			//	UpdateCurrentAbility();
			//}
		}

		public void UpdateCurrentAbility(Ability ability)
		{
			// Find ability by combination
			//currentAbility = abilities.Find(ability => CompareCombinations(ability.combination, currentCombination));
			currentAbility = ability;
			//currentCombination = new Enumerations.DropType[ElementsCount];

			// If ability is set then perform next changes
			if (currentAbility != null)
			{
				// Update UI
				abilityIconHolder.sprite = currentAbility.icon;
			}
			else
			{
				ResetElements();
			}

			// Auto ability use
			OnAbilityClick();
		}

		// This is just for development stage
		private IEnumerator TriggerRefuse()
		{
			Text refuseText = GameObject.Find("RefuseText").GetComponent<Text>();
			refuseText.enabled = true;

			yield return new WaitForSecondsRealtime(2f);

			refuseText.enabled = false;

			// Auto ability cleanup
			currentAbility = null;
			ResetElements();
			abilityIconHolder.sprite = defaultAbilityImage;
		}

		// Reset elements to their default state
		private void ResetElements()
		{
			foreach (Image elementObject in elementsImages)
			{
				elementObject.sprite = defaultElementImage;
			}
		}

		private bool CompareCombinations(IEnumerable<Enumerations.DropType> combinationOne, Enumerations.DropType[] combinationTwo)
		{
			Enumerations.DropType[] combo1 = combinationOne.OrderBy(drop => (byte)drop).ToArray();
			Enumerations.DropType[] combo2 = combinationTwo.OrderBy(drop => (byte)drop).ToArray();

			for (int i = 0; i < ElementsCount; i++)
			{
				if (combo1[i] != combo2[i])
				{
					return false;
				}
			}

			return true;
		}
	}
}
