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

	private Action continueCallback;

	private void Awake()
	{
		buttonContinue.onClick.AddListener(OnContinueClicked);
		SetVisible(false);
	}

	public void Show(int score, int highscore, bool isNewHighscore, Action onContinue)
	{
		SetVisible(true);

		this.score.text = score.ToCustomString();
		this.highscore.text = highscore.ToCustomString();

		if (isNewHighscore)
			highscoreRainbow.Animate();

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
