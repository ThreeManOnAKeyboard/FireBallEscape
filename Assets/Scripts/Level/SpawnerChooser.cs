using UnityEngine;

public class SpawnerChooser : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		// Activate respective spawner depending on choosen control type
		switch (GameManager.Instance.controlType)
		{
			case GameManager.ControlType.FREE:
				GetComponent<FreeControlSpawner>().enabled = true;
				break;
			case GameManager.ControlType.SIDEWAYS:
				GetComponent<LeftRightSpawner>().enabled = true;
				break;
			case GameManager.ControlType.ZIGZAG:
				GetComponent<ZigZagSpawner>().enabled = true;
				break;
		}
	}
}
