using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShieldController : MonoBehaviour
{
	// Shield parameters
	public int slotsCount = 1;
	public float rotationSpeed = 10f;
	public float lerpSpeed = 1f;
	public float maxAngle = 45f;

	// Change this to normal
	public Text counterText;

	//public SpriteRenderer[] shieldSprites;
	//public float effectDuration;

	//public SpriteRenderer collisionSpriteEffect;
	//public float collisionEffectDuration;

	//private SpriteRenderer spriteRenderer;
	private GameObject player;
	public static int leftSlots;
	private Vector3 previousPosition;

	private void Awake()
	{
		player = GameObject.FindWithTag(Tags.PLAYER);
		transform.parent = player.transform;
		transform.localPosition = Vector3.zero;
		//spriteRenderer = GetComponent<SpriteRenderer>();
	}

	// Use this for initialization
	private void OnEnable()
	{
		previousPosition = player.transform.position;
		transform.rotation = Quaternion.identity;
		leftSlots = slotsCount;
		counterText.text = leftSlots.ToString();
		//StartCoroutine(DoEffect(true));
		//StartCoroutine(DeactivateShield());
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
				ClampAngle
				(
					Mathf.LerpAngle
					(
						transform.eulerAngles.z,
						(previousPosition.x - player.transform.position.x) * rotationSpeed,
						Time.deltaTime * lerpSpeed
					)
				)
			)
		);

		// Save current position
		previousPosition = player.transform.position;
	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		if (--leftSlots == 0)
		{
			gameObject.SetActive(false);
		}

		counterText.text = leftSlots.ToString();

		//StartCoroutine(DoCollisionEffect(true));
	}

	//private IEnumerator DeactivateShield()
	//{
	//	yield return new WaitForSeconds(duration - blinkTimes * blinkDuration - effectDuration);

	//	// Blinking shield before deactivation
	//	for (int i = 0; i < blinkTimes; i++)
	//	{
	//		spriteRenderer.enabled = false;
	//		yield return new WaitForSeconds(blinkDuration / 2);

	//		spriteRenderer.enabled = true;
	//		yield return new WaitForSeconds(blinkDuration / 2);
	//	}

	//	StartCoroutine(DoEffect(false));

	//	yield return new WaitForSeconds(effectDuration);

	//	gameObject.SetActive(false);
	//}

	//private IEnumerator DoEffect(bool onActivation)
	//{
	//	float time = 0f;
	//	Color color;

	//	while (time <= effectDuration)
	//	{
	//		for (int i = 0; i < shieldSprites.Length; i++)
	//		{
	//			color = shieldSprites[i].color;
	//			color.a = onActivation ? (time / effectDuration) : (1f - time / effectDuration);
	//			shieldSprites[i].color = color;
	//		}

	//		time += Time.deltaTime;
	//		yield return null;
	//	}
	//}

	//private IEnumerator DoCollisionEffect(bool onEnable)
	//{
	//	float time = 0f;
	//	float duration = collisionEffectDuration / 2f;
	//	Color color;

	//	while (time <= duration)
	//	{
	//		color = collisionSpriteEffect.color;
	//		color.a = onEnable ? (time / duration) : (1 - time / duration);
	//		collisionSpriteEffect.color = color;

	//		time += Time.deltaTime;
	//		yield return null;
	//	}

	//	if (onEnable)
	//	{
	//		StartCoroutine(DoCollisionEffect(false));
	//	}
	//}

	private float ClampAngle(float angle)
	{
		if (angle < 0)
		{
			print(angle);
		}
		if (angle > maxAngle && angle < (360f - maxAngle))
		{
			if ((angle - maxAngle) < (360f - maxAngle - angle))
			{
				return maxAngle;
			}
			else
			{
				return 360f - maxAngle;
			}
		}
		else
		{
			return angle;
		}
	}
}
