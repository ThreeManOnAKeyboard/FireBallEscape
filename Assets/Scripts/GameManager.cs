using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }

	public enum ControlType
	{
		FREE,
		SIDEWAYS,
		ZIGZAG
	}
	public static ControlType controlType;

	void Awake()
	{
		Instance = this;
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

	public void EnableGameUI()
	{
		GameObject.FindWithTag(Tags.tags.GameUI.ToString()).GetComponent<Canvas>().enabled = true;
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
	}

	public void UnpauseGame()
	{
		Time.timeScale = 1;
	}

	public void RestartLevel()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void ExitApplication()
	{
		Application.Quit();
	}
}
