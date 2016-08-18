using System.Collections;
using UnityEngine;

public class FreeControlSpawner : Spawner
{
	public float additionalOffset;
	private float totalBorderOffset;

	// Use this for initialization
	new void Start()
	{
		base.Start();

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
			do
			{
				drop = ObjectPool.Instance.GetPooledObject(GetRandomDrop());
			} while (drop == null);

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
}
