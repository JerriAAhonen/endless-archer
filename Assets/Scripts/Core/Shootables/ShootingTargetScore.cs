using UnityEngine;

public class ShootingTargetScore : ShootingTargetBase
{
	[Header("Points")]
	[SerializeField] private int pointValue;
	[SerializeField] private string text;
	[SerializeField] private Color color;

	protected override void OnShot()
	{
		GameManager.Instance.LevelController.FloatingText.ShowText(transform.position + Vector3.up * labelOffset, text, color);
		GameManager.Instance.LevelController.Score.Add(pointValue);
	}
}
