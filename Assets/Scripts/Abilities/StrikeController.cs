using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeController : MonoBehaviour
{
	public GameObject fireBall;
	public int fireballsAmount = 1;
	public float fireRate = 1f;

	public static List<GameObject> targets;

	// Use this for initialization
	private void Awake()
	{
		targets = new List<GameObject>();
	}

	private void OnEnable()
	{
		StartCoroutine(SpawnFireBalls());
	}

	private IEnumerator SpawnFireBalls()
	{
		for (int i = 0; i < fireballsAmount; i++)
		{
			// Get instance
			GameObject fireBallInstance = ObjectPool.Instance.GetPooledObject(fireBall);

			// Activate fireball
			fireBallInstance.SetActive(true);

			yield return new WaitForSeconds(1f / fireRate);
		}

		// Deactivate ability when all fireballs reached their target
		while (GameObject.FindGameObjectsWithTag(Tags.FIREBALL).Length != 0)
		{
			yield return new WaitForSeconds(0.5f);
		}

		gameObject.SetActive(false);
	}
}
