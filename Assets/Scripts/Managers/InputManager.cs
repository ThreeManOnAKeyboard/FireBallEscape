using UnityEngine;

namespace Managers
{
	public class InputManager : MonoBehaviour
	{
		public static InputManager instance;

		private void Awake()
		{
			instance = this;
		}

		private void Update()
		{
			if (Input.GetKeyUp(KeyCode.Escape))
			{
				GameManager.Instance.PauseGame();
			}
		}
	}
}
