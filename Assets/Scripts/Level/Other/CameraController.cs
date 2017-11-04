using UnityEngine;
using _3rdParty;

namespace Level.Other
{
	public class CameraController : MonoBehaviour
	{
		public static CameraController instance;

		[Header("Camera Follow Parameters")]
		public float followSpeed;
		public float yPositionOffset;

		public float LeftBorder { get; private set; }
		public float RightBorder { get; private set; }

		private Transform target;

		private void Awake()
		{
			instance = this;
		}

		// Use this for initialization
		private void Start()
		{
			// Calculate visible track borders positions in world space coordinates
			target = GameObject.FindGameObjectWithTag(Tags.Player).transform;
			float distance = (target.position - Camera.main.transform.position).z;
			LeftBorder = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance)).x;
			RightBorder = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance)).x;
		}

		// Update is called once per frame
		private void Update()
		{
			Follow();
		}

		public void Follow()
		{
			if (target != null && Time.timeScale != 0 && Application.isPlaying)
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
	}
}
