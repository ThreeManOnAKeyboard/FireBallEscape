using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeControlSpawner : MonoBehaviour
{
	public List<DropHolder> drops;

	public float minSpawnInterval = 1f;
	public float maxSpawnInterval = 3f;

	public float additionalOffset;
	private float totalBorderOffset;

	private GameObject drop;

	// Use this for initialization
	void Start()
	{
		totalBorderOffset = GameManager.Instance.bordersOffset + additionalOffset;
		StartCoroutine(SpawnDrop());
	}

	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator SpawnDrop()
	{
		while (true)
		{
			// Wait the random amount of time till next drop instantiation
			yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

			// Pool new random drop
			while (drop == null)
			{
				drop = ObjectPool.Instance.GetPooledObject(GetRandomDrop());
			}

			drop.transform.position = new Vector3
			(
				Random.Range
				(
					CameraController.leftBorder + totalBorderOffset,
					CameraController.rightBorder - totalBorderOffset
				),
				transform.position.y,
				transform.position.z
			);
			drop.SetActive(true);
		}
	}

	public GameObject GetRandomDrop()
	{
		float randomResult = Random.Range(0, 1);

		for (int i = 0; i < drops.Count; i++)
		{
			if (drops[i].probability > randomResult)
			{
				return drops[i].drop;
			}
		}

		return null;
	}
}
