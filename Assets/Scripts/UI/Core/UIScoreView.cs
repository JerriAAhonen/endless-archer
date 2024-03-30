using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIScoreView : MonoBehaviour
{
	[SerializeField] private TextMeshProUGUI label;

	public void SetScore(int score)
	{
		label.text = score.ToCustomString();
	}
}
