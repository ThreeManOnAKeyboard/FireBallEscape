using System.Collections;
using UnityEngine;

public class SuperShieldController : MonoBehaviour
{
	public float duration;
	public float effectDuration;
	public float collisionEffectDuration;

	private PlayerController playerController;

	// Use this for initialization
	private void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();
		transform.parent = playerController.transform;
		transform.localPosition = Vector3.zero;
	}

	private void OnEnable()
	{
		playerController.isUnderSuperShield = true;
		StartCoroutine(DoEffect());
		StartCoroutine(DeactivateShield());
	}

	// Update is called once per frame
	private void Update()
	{

	}

	public void OnTriggerEnter2D(Collider2D col)
	{
		StartCoroutine(DoCollisionEffect());
	}

	private IEnumerator DeactivateShield()
	{
		yield return new WaitForSeconds(duration - effectDuration);

		StartCoroutine(DoEffect());

		yield return new WaitForSeconds(effectDuration);

		playerController.isUnderSuperShield = false;
		gameObject.SetActive(false);
	}

	private IEnumerator DoEffect()
	{
		yield return null;
	}

	private IEnumerator DoCollisionEffect()
	{
		yield return null;
	}
}
