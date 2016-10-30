using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public Text currentScoreText;
	public Text finalScoreText;
	public Text highScoreText;
	public float scoreRate = 1f;
	private int scoreMultiplier = 1;
	private float score = 0f;
	private Vector3 previousPosition;

	public static ScoreManager Instance;

	private Transform playerTransform;

	// Use this for initialization
	void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}

		playerTransform = GameObject.FindWithTag(Tags.PLAYER).transform;
	}

	// Update is called once per frame
	void Update()
	{
		if (playerTransform != null)
		{
			// Update the score
			score += (playerTransform.position.y - previousPosition.y) * scoreMultiplier * scoreRate;
			currentScoreText.text = ((int)score).ToString();

			// Save previous position for next frame calculations
			previousPosition = playerTransform.position;
		}
	}

	public void AddScore(float scoreAmount)
	{
		score += scoreAmount * scoreMultiplier++ * scoreRate;
	}

	public void AddMultiplier(int amount)
	{
		scoreMultiplier += amount;
		if (scoreMultiplier < 1)
		{
			scoreMultiplier = 0;
		}
	}

	public void ResetMultiplier()
	{
		if (!PlayerController.isConstHealth)
		{
			scoreMultiplier = 1;
		}
	}

	public float GetScore()
	{
		return score;
	}

	public void ProcessScore()
	{
		float currentHighScore = 0;

		if (PlayerPrefs.HasKey(PlayerPrefsKeys.HIGHSCORE))
		{
			currentHighScore = PlayerPrefs.GetFloat(PlayerPrefsKeys.HIGHSCORE);
		}

		if (score > currentHighScore)
		{
			PlayerPrefs.SetFloat(PlayerPrefsKeys.HIGHSCORE, score);
			currentHighScore = score;
		}

		finalScoreText.text = ((int)score).ToString();
		highScoreText.text = ((int)currentHighScore).ToString();
	}
}
