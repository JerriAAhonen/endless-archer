using UnityEngine;

public class ShootingTargetScore : ShootingTargetBase
{
	[Header("Points")]
	[SerializeField] private int pointValue;
	[SerializeField] private Color color;

	protected override void OnShot()
	{
		var args = new LevelController.ScoreArgs(
			pointValue, 
			transform.position, 
			transform.position + Vector3.up * labelOffset);
		GameManager.Instance.LevelController.AddScore(args);
	}
}
