using Managers;
using UnityEngine;
using _3rdParty;

namespace Level.Spawner
{
	public class SpawnerChooser : MonoBehaviour
	{
		// Use this for initialization
		private void Start()
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
}
