using UnityEngine;

public class TouchFollowMovement : MonoBehaviour
{
	public float speed;
	public float yOffset;
	[Range(0, 1)]
	public float yTouchLimitRatio;

	public Tags.tags cameraTag;

	private GameObject mainCamera;
	private Vector3 touchPosition;

	// Use this for initialization
	void Start()
	{
		mainCamera = GameObject.FindWithTag(cameraTag.ToString());
	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetMouseButton(0))
		{
			touchPosition = Camera.main.ScreenToWorldPoint
			(
				new Vector3
				(
					Input.mousePosition.x,
					Mathf.Clamp(Input.mousePosition.y, 0f, Screen.height * yTouchLimitRatio),
					Mathf.Abs(mainCamera.transform.position.z - transform.position.z)
				)
			);
		}
#elif UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			touchPosition = Camera.main.ScreenToWorldPoint
			(
				new Vector3
				(
					Input.GetTouch(0).position.x,
					Mathf.Clamp(Input.GetTouch(0).position.y, 0f, Screen.height * yTouchLimitRatio),
					Mathf.Abs(mainCamera.transform.position.z - transform.position.z)
				)
			);
		}
#endif

		transform.position = Vector3.Lerp
		(
			transform.position,
			new Vector3
			(
				touchPosition.x,
				touchPosition.y + yOffset,
				transform.position.z
			),
			Time.deltaTime * speed
		);
	}
}
