using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	[SerializeField] private LevelSegmentController segmentController;
	[SerializeField] private PlayerController playerController;
	[SerializeField] private FloatingTextController floatingTextController;
	[SerializeField] private UICoreController uiCoreController;

	private readonly float defaultComboDecayDuration = 2f;

	private ScoreController scoreController;

	public ScoreController Score => scoreController;
	public FloatingTextController FloatingText => floatingTextController;

	private void Start()
	{
		floatingTextController.Init(playerController.CameraTransform);
		uiCoreController.SetVisible(false);
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
		scoreController.Score.AddListener(OnAddScore);

		segmentController.StartLevel();
		playerController.OnStartLevel();
		uiCoreController.SetVisible(true);

		void OnAddScore(int newScore)
		{
			uiCoreController.Score.SetScore(newScore);
		}
	}

	public void GameOver()
	{
		playerController.OnLevelEnded();
		uiCoreController.SetVisible(false);
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
