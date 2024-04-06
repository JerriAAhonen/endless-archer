using TMPro;
using UnityEngine;

public class UIScoreView : UICoreViewBase
{
	[Space]
	[SerializeField] private TextMeshProUGUI scoreLabel;
	[SerializeField] private TextMeshProUGUI comboLabel;

	public void SetScore(int score)
	{
		scoreLabel.text = score.ToCustomString();
	}

	public void SetCombo(int combo)
	{
		comboLabel.text = $"{combo}x";
	}
}
