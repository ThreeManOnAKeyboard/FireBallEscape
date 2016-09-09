using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikeController : MonoBehaviour
{
	public GameObject fireBall;
	public int fireballsAmount = 1;
	public float cooldown = 1f;

	public static List<GameObject> targetedDrops;
	private GameObject player;

	// Use this for initialization
	void Awake()
	{
		targetedDrops = new List<GameObject>();
		player = GameObject.Find(Tags.tags.Player.ToString());
	}

	void OnEnable()
	{
		StartCoroutine(SpawnFireBalls());
	}

	private IEnumerator SpawnFireBalls()
	{
		for (int i = 0; i < fireballsAmount; i++)
		{
			GameObject fireBallInstance = ObjectPool.Instance.GetPooledObject(fireBall);
			if (player != null)
			{
				fireBallInstance.transform.position = player.transform.position;
			}
			fireBallInstance.SetActive(true);

			yield return new WaitForSeconds(cooldown);
		}

		// Deactivate ability when all fireballs reached their target
		while (GameObject.FindGameObjectsWithTag(Tags.tags.Fireball.ToString()).Length != 0)
		{
			yield return new WaitForSeconds(0.5f);
		}

		gameObject.SetActive(false);
	}
}
