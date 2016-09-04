using System.Collections;
using UnityEngine;

public class WaveBlastController : MonoBehaviour
{
	public float speed;

	private GameObject player;

	void Awake()
	{
		player = GameObject.FindWithTag(Tags.tags.Player.ToString());
	}

	// Use this for initialization
	void OnEnable()
	{
		transform.position = player.transform.position;
		StartCoroutine(SelfDestroy());
	}

	IEnumerator SelfDestroy()
	{
		while (Camera.main.WorldToViewportPoint(transform.position).y <= 1.3f && Camera.main.WorldToViewportPoint(transform.position).y >= -0.3f)
		{
			transform.Translate(Vector2.up * speed * Time.deltaTime);
			yield return null;
		}

		gameObject.SetActive(false);
	}
}
