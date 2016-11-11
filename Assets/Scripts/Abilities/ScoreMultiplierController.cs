using UnityEngine;
using System.Collections;

public class ScoreMultiplierController : AbilityController
{
	public float duration;

	private void OnEnable()
	{
		ScoreManager.Instance.AddMultiplier();
		StartCoroutine(SubstractMultiplier());
	}

	private IEnumerator SubstractMultiplier()
	{
		yield return new WaitForSeconds(duration);
		ScoreManager.Instance.SubstractMultiplier();
		gameObject.SetActive(false);
	}
}
