using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public enum ControlType
	{
		FREE,
		SIDEWAYS,
		ZIGZAG
	}
	public ControlType controlType;

	public enum PlayerPrefsKeys
	{
		RestartHappened,
		HighScore
	}

	// The x axis offset for both left / right borders to limit the player
	public float bordersOffset;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start()
	{
		// If the game was restarted then simulate play button press to get into "choose control type screen"
		if (PlayerPrefs.HasKey(PlayerPrefsKeys.RestartHappened.ToString()) && PlayerPrefs.GetInt(PlayerPrefsKeys.RestartHappened.ToString()) == 1)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.RestartHappened.ToString(), 0);
			ExecuteEvents.Execute(GameObject.Find("StartButton"), new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SetFreeControlType()
	{
		controlType = ControlType.FREE;
	}

	public void SetSidewaysControlType()
	{
		controlType = ControlType.SIDEWAYS;
	}

	public void SetZigZagControlType()
	{
		controlType = ControlType.ZIGZAG;
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
	}

	public void UnpauseGame()
	{
		Time.timeScale = 1;
	}

	public void GoToMenu()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void Restart()
	{
		PlayerPrefs.SetInt(PlayerPrefsKeys.RestartHappened.ToString(), 1);
		GoToMenu();
	}

	public void ExitApplication()
	{
		Application.Quit();
	}
}
