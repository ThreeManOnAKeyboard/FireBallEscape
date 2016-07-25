using UnityEngine;

public class Spawner : MonoBehaviour
{
	public Tags.tags caveBackgroundTag;
	GameObject caveBacground;
	float caveBackgroundHeight;
	string caveBackgroundName;

	public float offset;

	// Use this for initialization
	void Start()
	{
		caveBacground = GameObject.FindGameObjectWithTag(caveBackgroundTag.ToString());
		caveBackgroundHeight = caveBacground.GetComponent<MeshRenderer>().bounds.size.y - offset;
		caveBackgroundName = caveBacground.name;
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
}
