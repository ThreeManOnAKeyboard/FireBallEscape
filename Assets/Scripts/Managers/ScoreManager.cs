using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
	public static ScoreManager Instance;

	public Text currentScoreText;
	public Text finalScoreText;
	public Text highScoreText;
	public Text multiplierText;
	public float scoreRate;

	private float score;
	private int scoreMultiplier;

	private Vector3 previousPosition;
	private Transform playerTransform;

	// Use this for initialization
	private void Awake()
	{
		scoreMultiplier = 1;

		if (Instance == null)
		{
			Instance = this;
		}

		playerTransform = GameObject.FindWithTag(Tags.PLAYER).transform;
	}

	// Update is called once per frame
	private void Update()
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
		score += scoreAmount * scoreMultiplier * scoreRate;
	}

	public void AddMultiplier()
	{
		scoreMultiplier *= 2;
		multiplierText.text = scoreMultiplier.ToString();
	}

	public void SubstractMultiplier()
	{
		if (scoreMultiplier != 1)
		{
			scoreMultiplier /= 2;
			multiplierText.text = scoreMultiplier.ToString();
		}
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
