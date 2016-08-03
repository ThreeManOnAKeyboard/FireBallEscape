using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	public Tags.tags gameUITag;

	private float timeScale;

	void Awake()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start()
	{
		timeScale = Time.timeScale;
	}

	// Update is called once per frame
	void Update()
	{

	}

	public void EnableGameUI()
	{
		GameObject.FindWithTag(gameUITag.ToString()).GetComponent<Canvas>().enabled = true;
	}

	public void PauseGame()
	{
		Time.timeScale = 0f;
	}

	public void UnpauseGame()
	{
		Time.timeScale = timeScale;
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
