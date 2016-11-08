using System.Collections;
using UnityEngine;

public class ProtectionAuraController : AbilityController
{
	public GameObject collisionEffect;
	public float duration;

	private int currentQueue = 3001;

	private void Awake()
	{
		transform.parent = GameObject.FindWithTag(Tags.PLAYER).transform;
		transform.localPosition = Vector3.zero;
	}

	private void OnEnable()
	{
		StartCoroutine(Disable());
	}

	private IEnumerator Disable()
	{
		yield return new WaitForSecondsRealtime(duration);

		gameObject.SetActive(false);
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		GameObject effectInstance = ObjectPool.Instance.GetPooledObject(collisionEffect);
		Transform effectTransform = effectInstance.transform;
		effectTransform.parent = transform;
		effectTransform.localScale = Vector3.one;
		effectTransform.localPosition = Vector3.zero;
		effectTransform.LookAt(Vector3.forward);
		effectTransform.Rotate(Vector3.up * 270f);
		effectInstance.GetComponent<Renderer>().material.renderQueue = currentQueue - 1000;
		effectInstance.SetActive(true);

		// Draw collision effect on top of other effects
		if (currentQueue > 4000)
		{
			currentQueue = 3001;
		}
		else
		{
			++currentQueue;
		}
	}
}
