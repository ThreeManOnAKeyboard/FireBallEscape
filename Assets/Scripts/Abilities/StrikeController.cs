using System.Collections;
using UnityEngine;

public class StrikeController : MonoBehaviour
{
	public GameObject fireBall;
	public int fireballsAmount = 1;
	public float cooldown = 1f;

	private GameObject player;

	// Use this for initialization
	void Awake()
	{
		player = GameObject.Find(Tags.tags.Player.ToString());
	}

	// Update is called once per frame
	void Update()
	{
		// Follow the player
		transform.position = player.transform.position;
	}

	void OnEnable()
	{
		StartCoroutine(SpawnFireBalls());
	}

	private IEnumerator SpawnFireBalls()
	{
		for (int i = 0; i < fireballsAmount; i++)
		{
			ObjectPool.Instance.GetPooledObject(fireBall).SetActive(true);

			yield return new WaitForSeconds(cooldown);
		}

		gameObject.SetActive(false);
	}
}
