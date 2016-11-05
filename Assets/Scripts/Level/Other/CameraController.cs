using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;

	[Header("Camera Follow Parameters")]
	public float followSpeed;
	public float yPositionOffset;

	private float _leftBorder;
	private float _rightBorder;

	public float leftBorder { get { return _leftBorder; } }
	public float rightBorder { get { return _rightBorder; } }

	private Transform target;

	private void Awake()
	{
		Instance = this;

		// Enable the supported bloom
		Bloom thisBloom = GetComponent<Bloom>();
		if (thisBloom.enabled && !thisBloom.CheckResources())
		{
			thisBloom.enabled = false;

			GetComponent<BloomOptimized>().enabled = true;
		}

#if UNITY_ANDROID && !UNITY_EDITOR
		// Set Quality settings for android build
		QualitySettings.vSyncCount = 0;

		Application.targetFrameRate = 60;
#endif
	}

	// Use this for initialization
	private void Start()
	{
		// Calculate visible track borders positions in world space coordinates
		target = GameObject.FindGameObjectWithTag(Tags.PLAYER).transform;
		float distance = (target.position - Camera.main.transform.position).z;
		_leftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
		_rightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
	}

	// Update is called once per frame
	private void Update()
	{
		Follow();
	}

	public void Follow()
	{
		if (target != null)
		{
			transform.position = new Vector3
			(
				transform.position.x,
				Mathf.Lerp
				(
					transform.position.y,
					target.position.y + yPositionOffset,
					followSpeed * Time.unscaledDeltaTime
				),
				transform.position.z
			);
		}
	}

	public void EnableApplyRootMotion()
	{
		GetComponent<Animator>().applyRootMotion = true;
		GetComponent<CameraShake>().originZ = transform.position.z;
	}

	public void SetDownSample(int value)
	{
		GetComponent<BlurOptimized>().downsample = value;
	}
}
