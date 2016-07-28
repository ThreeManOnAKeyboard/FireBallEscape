using UnityEngine;

public class Spawner : MonoBehaviour
{
	public Tags.tags caveBackgroundTag;
	public GameObject waterDrop;
	public GameObject fuelDrop;
	private GameObject caveBacground;
	private float caveBackgroundHeight;
	private string caveBackgroundName;
	private Helper.methodNameHolder spawnWaterDropMethod;
	private Helper.methodNameHolder spawnFuelDropMethod;

	public float borderOffset;
	public float offset;
	public float minWaterSpawnInterval = 1f;
	public float maxWaterSpawnInterval = 3f;
	public float minFuelSpawnInterval = 1f;
	public float maxFuelSpawnInterval = 3f;

	// Use this for initialization
	void Start()
	{
		caveBacground = GameObject.FindGameObjectWithTag(caveBackgroundTag.ToString());
		caveBackgroundHeight = caveBacground.GetComponent<MeshRenderer>().bounds.size.y - offset;
		caveBackgroundName = caveBacground.name;
		spawnWaterDropMethod = new Helper.methodNameHolder(new Spawner().SpawnWaterDrop);
		spawnFuelDropMethod = new Helper.methodNameHolder(new Spawner().SpawnFuelDrop);
		Invoke(spawnWaterDropMethod.Method.Name, Random.Range(minWaterSpawnInterval, maxWaterSpawnInterval));
		Invoke(spawnFuelDropMethod.Method.Name, Random.Range(minFuelSpawnInterval, maxFuelSpawnInterval));
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position.y > caveBacground.transform.position.y)
		{
			caveBacground = Instantiate(caveBacground);
			caveBacground.transform.position = new Vector3
			(
				caveBacground.transform.position.x,
				caveBacground.transform.position.y + caveBackgroundHeight,
				caveBacground.transform.position.z
			);
			caveBacground.name = caveBackgroundName;
		}
	}

	void SpawnWaterDrop()
	{
		Instantiate
		(
			waterDrop,
			new Vector3
			(
				Random.Range(CameraController.leftBorder + borderOffset, CameraController.rightBorder - borderOffset),
				transform.position.y,
				transform.position.z
			),
			Quaternion.identity
		);

		Invoke(spawnWaterDropMethod.Method.Name, Random.Range(minWaterSpawnInterval, maxWaterSpawnInterval));
	}

	void SpawnFuelDrop()
	{
		Instantiate
		(
			fuelDrop,
			new Vector3
			(
				Random.Range(CameraController.leftBorder + borderOffset, CameraController.rightBorder - borderOffset),
				transform.position.y,
				transform.position.z
			),
			Quaternion.identity
		);

		Invoke(spawnFuelDropMethod.Method.Name, Random.Range(minFuelSpawnInterval, maxFuelSpawnInterval));
	}
}
