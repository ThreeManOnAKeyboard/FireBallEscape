using System.Collections;
using UnityEngine;

public class ZigZagSpawner : MonoBehaviour
{
	public GameObject waterDrop;
	public GameObject fuelDrop;

	public float minSpawnInterval = 1f;
	public float maxSpawnInterval = 3f;

	// How much fuel should be spawned per water drop
	[Range(0, 1)]
	public float fuelPerWaterSpawnRatio;

	public float additionalOffset;
	private float totalBorderOffset;

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

			// Instantiate new random drop
			Instantiate
			(
				Random.Range(0f, 1f) < fuelPerWaterSpawnRatio ? fuelDrop : waterDrop,
				new Vector3
				(
					Random.Range
					(
						CameraController.leftBorder + totalBorderOffset,
						CameraController.rightBorder - totalBorderOffset
					),
					transform.position.y,
					transform.position.z
				),
				Quaternion.identity
			);
		}
	}
}
