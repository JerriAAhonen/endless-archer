using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
	private readonly float defaultComboDecayDuration = 2f;

	private ScoreController scoreController;

	public void StartLevel()
	{
		scoreController = new ScoreController(defaultComboDecayDuration);
	}

	public void GameOver()
	{
		// Save highscore
	}
}
