using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneRainController : MonoBehaviour
{
	public List<DropHolder> stoneRainDrops;
	public float duration;

	public static bool isActive;

	private delegate void EnableMethod(List<DropHolder> stoneRainDrops);
	private EnableMethod enableMethod;

	private delegate void ResetMethod();
	private ResetMethod resetMethod;

	// Use this for initialization
	private void Awake()
	{
		switch (GameManager.Instance.controlType)
		{
			case GameManager.ControlType.FREE:
				enableMethod = FindObjectOfType<FreeControlSpawner>().ChangeCurrentDrops;
				resetMethod = FindObjectOfType<FreeControlSpawner>().ResetDrops;
				break;
			case GameManager.ControlType.SIDEWAYS:
				enableMethod = FindObjectOfType<LeftRightSpawner>().ChangeCurrentDrops;
				resetMethod = FindObjectOfType<LeftRightSpawner>().ResetDrops;
				break;
			case GameManager.ControlType.ZIGZAG:
				enableMethod = FindObjectOfType<ZigZagSpawner>().ChangeCurrentDrops;
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

		isActive = true;
		enableMethod(stoneRainDrops);

		StartCoroutine(ResetDrops());
	}

	private void OnDisable()
	{
		isActive = false;
	}

	private IEnumerator ResetDrops()
	{
		yield return new WaitForSeconds(duration);

		resetMethod();
		gameObject.SetActive(false);
	}
}
