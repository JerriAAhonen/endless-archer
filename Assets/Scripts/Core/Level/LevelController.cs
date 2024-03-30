using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] private LevelSegmentController segmentController;
	[SerializeField] private PlayerController playerController;
	[SerializeField] private FloatingTextController floatingTextController;

	private readonly float defaultComboDecayDuration = 2f;

	private ScoreController scoreController;

	public ScoreController Score => scoreController;
	public FloatingTextController FloatingText => floatingTextController;

	private void Start()
	{
		floatingTextController.Init(playerController.CameraTransform);
	}

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
