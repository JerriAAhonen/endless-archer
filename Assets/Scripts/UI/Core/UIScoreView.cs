using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreView : MonoBehaviour
{
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
