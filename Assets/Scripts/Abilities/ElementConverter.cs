using UnityEngine;
using System.Collections;

public class ElementConverter : AbilityController
{
	public float duration;
	public GameObject fuelDropPrefab;

	private Transform playerTransform;

	// Use this for initialization
	private void Awake()
	{
		playerTransform = FindObjectOfType<PlayerController>().transform;
	}

	private void OnEnable()
	{
		StartCoroutine(DisableAfterDelay());
	}

	// Update is called once per frame
	private void Update()
	{
		if (playerTransform != null)
		{
			transform.position = playerTransform.position;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		GameObject fuelDrop = ObjectPool.Instance.GetPooledObject(fuelDropPrefab);
		fuelDrop.transform.position = col.transform.position;
		fuelDrop.SetActive(true);
	}

	private IEnumerator DisableAfterDelay()
	{
		yield return new WaitForSeconds(duration);

		gameObject.SetActive(false);
	}
}
