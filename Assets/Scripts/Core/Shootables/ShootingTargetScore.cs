using UnityEngine;

public class ShootingTargetScore : ShootingTargetBase
{
	[Header("Points")]
	[SerializeField] private int pointValue;
	[SerializeField] private Color color;

	protected override void OnShot()
	{
		var args = new LevelController.ScoreArgs
		{
			amount = pointValue,
			targetPosition = transform.position,
			floatingTextPos = transform.position + Vector3.up * labelOffset,
			floatingTextColor = color
		};
		GameManager.Instance.LevelController.AddScore(args);
	}
}
