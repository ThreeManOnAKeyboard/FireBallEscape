using UnityEngine;
using System.Collections;

public class ElementConverterController : MonoBehaviour
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

	}

	private void OnDisable()
	{

	}

	// Update is called once per frame
	private void Update()
	{
		transform.position = playerTransform.position;
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		GameObject fuelDrop = ObjectPool.Instance.GetPooledObject(fuelDropPrefab);
		fuelDrop.transform.position = col.transform.position;
	}
}
