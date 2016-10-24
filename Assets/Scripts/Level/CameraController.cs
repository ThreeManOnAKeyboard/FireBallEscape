using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class CameraController : MonoBehaviour
{
	public static CameraController Instance;

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
		if (target != null)
		{
			transform.position = new Vector3
			(
				transform.position.x,
				Mathf.Lerp(transform.position.y, target.position.y + yPositionOffset, followSpeed * Time.deltaTime),
				transform.position.z
			);
		}
	}

	public void Shake(float duration, float intensity)
	{
		StartCoroutine(PerformShake(duration, intensity));
	}

	private IEnumerator PerformShake(float duration, float intensity)
	{
		float time = 0f;

		while (time < duration)
		{
			// Cutremur

			time += Time.deltaTime;
			yield return null;
		}
	}

	public void EnableApplyRootMotion()
	{
		GetComponent<Animator>().applyRootMotion = true;
	}

	public void SetDownSample(int value)
	{
		GetComponent<BlurOptimized>().downsample = value;
	}
}
