using System.Collections;
using UnityEngine;

public class ProtectionAuraController : AbilityController
{
	public float duration;
	public float maxAngleRange;

	private void Awake()
	{
		transform.parent = GameObject.FindWithTag(Tags.PLAYER).transform;
		transform.localPosition = Vector3.zero;
	}

	private void OnEnable()
	{
		StartCoroutine(Disable());
	}

	private void Update()
	{
		//transform.rotation = Quaternion.Euler
		//(

		//);
	}

	private IEnumerator Disable()
	{
		yield return new WaitForSeconds(duration);

		gameObject.SetActive(false);
	}
}
