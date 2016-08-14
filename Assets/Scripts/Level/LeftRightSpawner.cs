using System.Collections;
using UnityEngine;

public class LeftRightSpawner : MonoBehaviour
{
	public GameObject waterDrop;
	public GameObject fuelDrop;
	public float minSpawnInterval = 0.3f;
	public float maxSpawnInterval = 1f;

	// How much fuel should be spawned per water drop
	[Range(0, 1)]
	public float fuelPerWaterSpawnRatio;

	// The script which contains the calculated positions of bands
	LeftRightMovement LRMScript;

	// Use this for initialization
	void Start()
	{
		LRMScript = FindObjectOfType<LeftRightMovement>();

		// Start for each band it's own coroutine
		for (int i = 0; i < LRMScript.bandsPositions.Length; i++)
		{
			StartCoroutine(SpawnDrop(LRMScript.bandsPositions[i]));
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	IEnumerator SpawnDrop(float spawnPosition)
	{
		while (true)
		{
			// Wait the random amount of time till next drop instantiation
			yield return new WaitForSeconds(Random.Range(minSpawnInterval, maxSpawnInterval));

			// Instantiate new random drop
			Instantiate
			(
				Random.Range(0f, 1f) < fuelPerWaterSpawnRatio ? fuelDrop : waterDrop,
				new Vector3
				(
					spawnPosition,
					transform.position.y,
					transform.position.z
				),
				Quaternion.identity
			);
		}
	}
}
