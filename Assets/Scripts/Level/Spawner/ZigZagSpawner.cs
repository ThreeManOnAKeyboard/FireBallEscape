using System.Collections;
using UnityEngine;

public class ZigZagSpawner : Spawner
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

	private IEnumerator SpawnDrop()
	{
		while (true)
		{
			// Wait the random amount of time till next drop instantiation
			yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

			// Pool new random drop
			do
			{
				objectToSpawn = ObjectPool.Instance.GetPooledObject(GetRandomSpawnable());
			} while (objectToSpawn == null);

			objectToSpawn.transform.position = new Vector3
			(
				Random.Range
				(
					CameraController.Instance.leftBorder + totalBorderOffset,
					CameraController.Instance.rightBorder - totalBorderOffset
				),
				transform.position.y,
				transform.position.z
			);
			objectToSpawn.SetActive(true);
		}
	}
}
