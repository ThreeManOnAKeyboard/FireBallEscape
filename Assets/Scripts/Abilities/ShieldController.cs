using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
	// Shield parameters
	public float shieldDuration;
	public float rotationSpeed = 10f;
	public float lerpSpeed = 1f;
	[Range(0.05f, 1f)]
	public float blinkDuration = 0.5f;
	public int blinkTimes = 3;

	private SpriteRenderer spriteRenderer;
	private GameObject player;
	private Vector3 previousPosition;

	void Awake()
	{
		player = GameObject.Find(Tags.tags.Player.ToString());
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	void OnEnable()
	{
		previousPosition = transform.position;
		StartCoroutine(DeactivateShield());
	}

	// Update is called once per frame
	void Update()
	{
		// Verify if player is alive
		if (player == null)
		{
			gameObject.SetActive(false);
			return;
		}

		// Follow the player
		transform.position = player.transform.position;

		// Update the rotation angle of shield
		transform.rotation = Quaternion.Euler
		(
			new Vector3
			(
				transform.eulerAngles.x,
				transform.eulerAngles.y,
				Mathf.LerpAngle
				(
					transform.eulerAngles.z,
					(previousPosition.x - transform.position.x) * rotationSpeed,
					Time.deltaTime * lerpSpeed
				)
			)
		);

		// Save current position
		previousPosition = transform.position;
	}

	IEnumerator DeactivateShield()
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
