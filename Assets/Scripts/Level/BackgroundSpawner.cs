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

	// Use this for initialization
	void Start()
	{
		caveBackgroundHeight = caveBackground.GetComponent<MeshRenderer>().bounds.size.y - offset;
		caveBackgroundName = caveBackground.name;

	}

	// Update is called once per frame
	void Update()
	{
		if (transform.position.y > caveBackground.transform.position.y)
		{
			caveBackground = Instantiate(caveBackground);
			caveBackground.transform.position = new Vector3
			(
				caveBackground.transform.position.x,
				caveBackground.transform.position.y + caveBackgroundHeight,
				caveBackground.transform.position.z
			);
			caveBackground.name = caveBackgroundName;
		}
	}
}
