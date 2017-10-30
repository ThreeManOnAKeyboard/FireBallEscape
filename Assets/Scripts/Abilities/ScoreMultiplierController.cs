using System.Collections;
using Managers;
using UnityEngine;

namespace Abilities
{
	public class ScoreMultiplierController : AbilityController
	{
		public float duration;

		private void OnEnable()
		{
			ScoreManager.instance.AddMultiplier();
			StartCoroutine(SubstractMultiplier());
		}

		private IEnumerator SubstractMultiplier()
		{
			yield return new WaitForSeconds(duration);
			ScoreManager.instance.SubstractMultiplier();
			gameObject.SetActive(false);
		}
	}
}
