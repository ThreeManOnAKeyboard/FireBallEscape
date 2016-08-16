using UnityEngine;

public class BackgroundSpawner : MonoBehaviour
{
	// The cave mesh that will be generated in front of player
	public GameObject caveBackground;

	// The offset in units between generated cave objects
	public float offset;

	// Some data used when generating the caves
	private float caveBackgroundHeight;
	private string caveBackgroundName;

	private Vector3 previousCavePosition;
	private GameObject cave;

	// Use this for initialization
	void Start()
	{
		caveBackgroundHeight = caveBackground.GetComponent<MeshRenderer>().bounds.size.y - offset;
		caveBackgroundName = caveBackground.name;
		cave = caveBackground;
	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position.y > cave.transform.position.y)
		{
			previousCavePosition = cave.transform.position;
			cave = ObjectPool.Instance.GetPooledObject(caveBackground);
			cave.transform.position = new Vector3
			(
				previousCavePosition.x,
				previousCavePosition.y + caveBackgroundHeight,
				previousCavePosition.z
			);
			cave.SetActive(true);
		}
	}
}
