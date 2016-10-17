using UnityEngine;

public class SpawnerChooser : MonoBehaviour
{
	// Use this for initialization
	void Start()
	{
		// Activate respective spawner depending on choosen control type
		switch (GameManager.Instance.controlType)
		{
			case Enumerations.ControlType.Free:
				GetComponent<FreeControlSpawner>().enabled = true;
				break;
			case Enumerations.ControlType.Sideways:
				GetComponent<LeftRightSpawner>().enabled = true;
				break;
			case Enumerations.ControlType.ZigZag:
				GetComponent<ZigZagSpawner>().enabled = true;
				break;
		}
	}
}
