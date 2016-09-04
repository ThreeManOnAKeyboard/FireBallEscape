using UnityEngine;

public class ShieldController : MonoBehaviour
{
	// Shield parameters
	public float shieldDuration;
	public float rotationSpeed = 10f;
	public float lerpSpeed = 1f;

	private GameObject player;

	void Awake()
	{
		player = GameObject.Find(Tags.tags.Player.ToString());
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// Update the rotation angle of character
		//transform.eulerAngles = new Vector3
		//(
		//	transform.eulerAngles.x,
		//	transform.eulerAngles.y,
		//	Mathf.LerpAngle
		//	(
		//		transform.eulerAngles.z,
		//		(previousPosition.x - transform.position.x) * degreesPerUnit,
		//		Time.deltaTime * rotationSpeed
		//	)
		//);
	}
}
