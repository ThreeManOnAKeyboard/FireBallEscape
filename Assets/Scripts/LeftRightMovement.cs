using UnityEngine;

public class LeftRightMovement : MonoBehaviour
{
	public Vector2 speed;
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
					Input.mousePosition.y,
					Mathf.Abs(mainCamera.transform.position.z - transform.position.z)
				)
			);
		}
		else
		{
			touchPosition = transform.position;
		}
#elif UNITY_ANDROID
		if (Input.touchCount > 0)
		{
			if (Input.GetTouch(0).phase == TouchPhase.Ended)
			{
				touchPosition = transform.position;
			}
			else
			{
				touchPosition = Camera.main.ScreenToWorldPoint
				(
					new Vector3
					(
						Input.GetTouch(0).position.x,
						Input.GetTouch(0).position.y,
						Mathf.Abs(mainCamera.transform.position.z - transform.position.z)
					)
				);
			}
		}
#endif

		transform.position = new Vector3
		(
			Mathf.Lerp(transform.position.x, touchPosition.x, Time.deltaTime * speed.x),
			transform.position.y,
			transform.position.z
		);

		transform.Translate(Vector2.up * speed.y * Time.deltaTime);
	}
}
