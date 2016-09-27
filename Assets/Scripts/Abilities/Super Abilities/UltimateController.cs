using System.Collections;
using UnityEngine;

public class UltimateController : MonoBehaviour
{
	public float duration;
	public float speed;

	private PlayerController playerController;

	// Use this for initialization
	private void Awake()
	{
		playerController = FindObjectOfType<PlayerController>();
	}

	private void OnEnable()
	{
		playerController.ActivateUltimate(speed);
		StartCoroutine(DeactivateUltimate());
	}

	private IEnumerator DeactivateUltimate()
	{
		yield return new WaitForSeconds(duration);
		playerController.DeactivateUltimate();
		gameObject.SetActive(false);
	}
}
