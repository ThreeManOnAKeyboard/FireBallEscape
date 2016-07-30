using UnityEngine;

public class TouchFollowMovement : MonoBehaviour
{
	public float speed;
	private Vector3 touchPosition;

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
		if (Input.GetMouseButton(0))
		{
			touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 8f));
			print(touchPosition);
		}
#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            touchPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 10f));
        }
#endif

		transform.position = Vector3.Lerp
		(
			transform.position,
			new Vector3
			(
				touchPosition.x,
				touchPosition.y,
				transform.position.z
			),
			Time.deltaTime * speed
		);
	}
}
