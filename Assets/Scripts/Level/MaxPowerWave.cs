using System.Collections;
using UnityEngine;

public class MaxPowerWave : MonoBehaviour
{
	public float speed;

	void Start()
	{
		StartCoroutine(SelfDestroy());
	}

	IEnumerator SelfDestroy()
	{
		while (Camera.main.WorldToViewportPoint(transform.position).y <= 1.3f)
		{
			transform.Translate(Vector2.up * speed * Time.deltaTime);
			yield return null;
		}

		Destroy(gameObject);
	}
}
