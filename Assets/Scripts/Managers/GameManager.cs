﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using _3rdParty;

namespace Managers
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager Instance { get; private set; }

		[HideInInspector]
		public Enumerations.ControlType controlType;

		// The x axis offset for both left / right borders to limit the player
		public float bordersOffset;

		private float currentTimeScale;

		private void Awake()
		{
			if (Instance == null)
			{
				Instance = this;
			}

			// Uncomment this if you want to reset highscores and stuff like that
			//PlayerPrefs.DeleteAll();
		}

		// Use this for initialization
		private void Start()
		{
			// If the game was restarted then simulate play button press to get into "choose control type screen"
			if (PlayerPrefs.HasKey(PlayerPrefsKeys.RestartHappened) && PlayerPrefs.GetInt(PlayerPrefsKeys.RestartHappened) == 1)
			{
				PlayerPrefs.SetInt(PlayerPrefsKeys.RestartHappened, 0);
				ExecuteEvents.Execute(GameObject.Find("StartButton"), new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
			}
		}

		public void SetFreeControlType()
		{
			controlType = Enumerations.ControlType.Free;
		}

		public void SetSidewaysControlType()
		{
			controlType = Enumerations.ControlType.Sideways;
		}

		public void SetZigZagControlType()
		{
			controlType = Enumerations.ControlType.ZigZag;
		}

		public void PauseGame()
		{
			currentTimeScale = Time.timeScale;
			Time.timeScale = 0f;
		}

		public void UnpauseGame()
		{
			Time.timeScale = currentTimeScale;
		}

		public void GoToMenu()
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().name);
		}

		public void Restart()
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.RestartHappened, 1);
			GoToMenu();
		}

		public void ExitApplication()
		{
#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
		}
	}
}
