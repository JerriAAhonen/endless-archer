using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverView : UICoreViewBase
{
	[Space]
	[SerializeField] private TextMeshProUGUI score;
	[SerializeField] private TextMeshProUGUI highscore;
	[SerializeField] private UIRainbowText highscoreRainbow;
	[SerializeField] private Button buttonContinue;
	[Space]
	[SerializeField] private float scoreAnimDur = 1f;
	[SerializeField] private float newHighscoreAnimDurMultiplier = 1.5f;

	private Action continueCallback;

	private void Awake()
	{
		buttonContinue.onClick.AddListener(OnContinueClicked);
		SetVisible(false);
	}

	public void Show(int score, int highscore, bool isNewHighscore, Action onContinue)
	{
		SetVisible(true);

		this.score.text = "0";
		this.highscore.text = "0";
		UIAnimUtil.AnimateNumber(this.score, 0, score, scoreAnimDur);
		var highscoreAnimDur = isNewHighscore ? scoreAnimDur * newHighscoreAnimDurMultiplier : scoreAnimDur;
		UIAnimUtil.AnimateNumber(this.highscore, 0, highscore, highscoreAnimDur, () =>
		{
			if (isNewHighscore)
				highscoreRainbow.Animate();
		});

		continueCallback = onContinue;
	}

	private void OnContinueClicked()
	{
		continueCallback?.Invoke();
		continueCallback = null;
		SetVisible(false);
		highscoreRainbow.Stop();
	}
}
