using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FuelRainController : MonoBehaviour
{
	public List<DropHolder> fuelRainDrops;
	public float fuelRainDuration;

	private delegate void OnEnableMethod(List<DropHolder> fuelRainDrops);
	private OnEnableMethod onEnableMethod;

	private delegate void ResetMethod();
	private ResetMethod resetMethod;

	// Use this for initialization
	void Awake()
	{
		switch (GameManager.Instance.controlType)
		{
			case GameManager.ControlType.FREE:
				onEnableMethod = FindObjectOfType<FreeControlSpawner>().ActivateFuelDropRain;
				resetMethod = FindObjectOfType<FreeControlSpawner>().ResetDrops;
				break;
			case GameManager.ControlType.SIDEWAYS:
				onEnableMethod = FindObjectOfType<LeftRightSpawner>().ActivateFuelDropRain;
				resetMethod = FindObjectOfType<LeftRightSpawner>().ResetDrops;
				break;
			case GameManager.ControlType.ZIGZAG:
				onEnableMethod = FindObjectOfType<ZigZagSpawner>().ActivateFuelDropRain;
				resetMethod = FindObjectOfType<ZigZagSpawner>().ResetDrops;
				break;
		}
	}

	void OnEnable()
	{
		onEnableMethod(fuelRainDrops);

		StartCoroutine(ResetDrops());
	}

	private IEnumerator ResetDrops()
	{
		yield return new WaitForSeconds(fuelRainDuration);

		resetMethod();
		gameObject.SetActive(false);
	}
}
