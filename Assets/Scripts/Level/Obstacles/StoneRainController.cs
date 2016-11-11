using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StoneRainController : MonoBehaviour
{
	[Header("Camera Shake Parameters")]
	public float shakeDuration;
	public float shakeSpeed;
	public float shakeMagnitude;
	public float zoomDistance;

	[Header("Earth Shake Parameters")]
	public List<DropSpawnProperties> stoneRainDrops;
	public float duration;

	public static bool isActive;

	private Spawner currentSpawner;

	// Use this for initialization
	private void Awake()
	{
		currentSpawner = GameObject.FindWithTag(Tags.SPAWNER).GetComponents<Spawner>().Single(spawner => spawner.enabled);
	}

	private void OnEnable()
	{
		isActive = true;
		StartCoroutine(StartEarthshake());
	}

	private IEnumerator StartEarthshake()
	{
		CameraShake.Instance.StartShake(shakeDuration, shakeSpeed, shakeMagnitude, zoomDistance);

		yield return new WaitForSeconds(shakeDuration);

		currentSpawner.ChangeSpawnables(stoneRainDrops);
		Spawner.canChangeSpawnables = false;
		StartCoroutine(ResetDrops());
	}

	private IEnumerator ResetDrops()
	{
		yield return new WaitForSeconds(duration);

		currentSpawner.ResetSpawnables();
		Spawner.canChangeSpawnables = true;
		isActive = false;
		gameObject.SetActive(false);
	}
}
