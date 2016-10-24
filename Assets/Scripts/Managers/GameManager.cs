using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public Enumerations.ControlType controlType;

	// The x axis offset for both left / right borders to limit the player
	public float bordersOffset;

	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		// Uncomment this if you want to reset highscores and stuff like that
		//PlayerPrefs.DeleteAll();
	}

	// Use this for initialization
	void Start()
	{
		// If the game was restarted then simulate play button press to get into "choose control type screen"
		if (PlayerPrefs.HasKey(PlayerPrefsKeys.RESTART_HAPPENED) && PlayerPrefs.GetInt(PlayerPrefsKeys.RESTART_HAPPENED) == 1)
		{
			PlayerPrefs.SetInt(PlayerPrefsKeys.RESTART_HAPPENED, 0);
			ExecuteEvents.Execute(GameObject.Find("StartButton"), new PointerEventData(EventSystem.current), ExecuteEvents.submitHandler);
		}
	}

	// Update is called once per frame
	void Update()
	{

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
		PlayerPrefs.SetInt(PlayerPrefsKeys.RESTART_HAPPENED, 1);
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
