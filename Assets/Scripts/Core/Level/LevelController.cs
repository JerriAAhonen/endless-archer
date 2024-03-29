using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] private LevelSegmentController segmentController;
	[SerializeField] private PlayerController playerController;

	private readonly float defaultComboDecayDuration = 2f;

	private ScoreController scoreController;

	public ScoreController Score => scoreController;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.F))
		{
			GameOver();
		}
	}

	public void StartLevel()
	{
		scoreController = new ScoreController(defaultComboDecayDuration);
		segmentController.StartLevel();

		playerController.OnStartLevel();
	}

	public void GameOver()
	{
		playerController.OnLevelEnded();
		CheckHighscore();
		GameManager.Instance.OnLevelEnded();
		
		void CheckHighscore()
		{
			var oldHighscore = PlayerPrefsUtil.Highscore;
			if (scoreController.Score > oldHighscore)
			{
				PlayerPrefsUtil.Highscore = scoreController.Score;
				PlayerPrefsUtil.HighscoreTime = scoreController.TimeElapsed;
			}
		}
	}
}
