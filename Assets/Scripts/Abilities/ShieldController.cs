using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
	// Shield parameters
	public float shieldDuration;
	public float rotationSpeed = 10f;
	public float lerpSpeed = 1f;
	public float maxAngle = 45f;
	[Range(0.05f, 1f)]
	public float blinkDuration = 0.5f;
	public int blinkTimes = 3;

	private SpriteRenderer spriteRenderer;
	private GameObject player;
	private Vector3 previousPosition;

	private void Awake()
	{
		player = GameObject.Find(Tags.tags.Player.ToString());
		transform.parent = player.transform;
		transform.localPosition = Vector3.zero;
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	private void OnEnable()
	{
		previousPosition = player.transform.position;
		StartCoroutine(DeactivateShield());
	}

	// Update is called once per frame
	private void Update()
	{
		// Verify if player is alive
		if (player == null)
		{
			gameObject.SetActive(false);
			return;
		}

		// Update the rotation angle of shield
		transform.rotation = Quaternion.Euler
		(
			new Vector3
			(
				transform.eulerAngles.x,
				transform.eulerAngles.y,
					//Mathf.Clamp
					//(
					Mathf.LerpAngle
					(
						transform.eulerAngles.z,
						(previousPosition.x - player.transform.position.x) * rotationSpeed,
						Time.deltaTime * lerpSpeed
					)
			//	-maxAngle,
			//	maxAngle
			//)
			)
		);

		// Save current position
		previousPosition = player.transform.position;
	}

	private IEnumerator DeactivateShield()
	{
		yield return new WaitForSeconds(shieldDuration - blinkTimes * blinkDuration);

		// Blinking shield before deactivation
		for (int i = 0; i < blinkTimes; i++)
		{
			spriteRenderer.enabled = false;
			yield return new WaitForSeconds(blinkDuration / 2);

			spriteRenderer.enabled = true;
			yield return new WaitForSeconds(blinkDuration / 2);
		}

		gameObject.SetActive(false);
	}
}
