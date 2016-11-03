using System.Collections;
using System.Collections.Generic;
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

	private delegate void EnableMethod(List<DropSpawnProperties> stoneRainDrops);
	private EnableMethod enableMethod;

	private delegate void ResetMethod();
	private ResetMethod resetMethod;

	// Use this for initialization
	private void Awake()
	{
		switch (GameManager.Instance.controlType)
		{
			case Enumerations.ControlType.Free:
				enableMethod = FindObjectOfType<FreeControlSpawner>().ChangeSpawnables;
				resetMethod = FindObjectOfType<FreeControlSpawner>().ResetDrops;
				break;
			case Enumerations.ControlType.Sideways:
				enableMethod = FindObjectOfType<LeftRightSpawner>().ChangeSpawnables;
				resetMethod = FindObjectOfType<LeftRightSpawner>().ResetDrops;
				break;
			case Enumerations.ControlType.ZigZag:
				enableMethod = FindObjectOfType<ZigZagSpawner>().ChangeSpawnables;
				resetMethod = FindObjectOfType<ZigZagSpawner>().ResetDrops;
				break;
		}
	}

	private void OnEnable()
	{
		if (FindObjectOfType<FuelRainController>() != null)
		{
			gameObject.SetActive(false);
		}

		StartCoroutine(StartEarthshake());
	}

	private void OnDisable()
	{
		isActive = false;
	}

	private IEnumerator StartEarthshake()
	{
		CameraShake.Instance.StartShake(shakeDuration, shakeSpeed, shakeMagnitude, zoomDistance);

		yield return new WaitForSeconds(shakeDuration);

		enableMethod(stoneRainDrops);
		StartCoroutine(ResetDrops());
		isActive = true;
	}

	private IEnumerator ResetDrops()
	{
		yield return new WaitForSeconds(duration);

		resetMethod();
		gameObject.SetActive(false);
	}
}
