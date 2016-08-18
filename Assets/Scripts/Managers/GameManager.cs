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

	// Highscore text elements
	public Text highScore;
	public Text score;

	void Awake()
	{
		Instance = this;
		//PlayerPrefs.DeleteAll();
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

	public void ProcessScore()
	{
		float currentHighScore = 0;

		if (PlayerPrefs.HasKey(PlayerPrefsKeys.HighScore.ToString()))
		{
			currentHighScore = PlayerPrefs.GetFloat(PlayerPrefsKeys.HighScore.ToString());
		}

		if (PlayerController.score > currentHighScore)
		{
			PlayerPrefs.SetFloat(PlayerPrefsKeys.HighScore.ToString(), PlayerController.score);
			currentHighScore = PlayerController.score;
		}

		score.text = ((int)PlayerController.score).ToString();
		highScore.text = ((int)currentHighScore).ToString();
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
