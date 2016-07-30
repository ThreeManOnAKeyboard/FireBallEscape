using UnityEngine;

public class CameraController : MonoBehaviour
{
	public static float leftBorder;
	public static float rightBorder;

	public Tags.tags playerTag;
	public Tags.tags backgroundTag;

	Transform target;

	public float followSpeed;
	public float yPositionOffset;

	void Awake()
	{
#if UNITY_ANDROID && !UNITY_EDITOR
		QualitySettings.vSyncCount = 0;
		Application.targetFrameRate = 50;
#endif
	}

	// Use this for initialization
	void Start()
	{
		target = GameObject.FindGameObjectWithTag(playerTag.ToString()).transform;
		float distance = (target.position - Camera.main.transform.position).z;
		leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
		rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
	}

	// Update is called once per frame
	void Update()
	{
		transform.position = new Vector3
		(
			transform.position.x,
			Mathf.Lerp(transform.position.y, target.position.y + yPositionOffset, Time.deltaTime * followSpeed),
			transform.position.z
		);
	}
}
