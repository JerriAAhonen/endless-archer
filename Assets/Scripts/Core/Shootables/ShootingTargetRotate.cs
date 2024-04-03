using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTargetRotate : ShootingTargetBase
{
	[Header("Rotation Direction")]
	[SerializeField] private bool clockWise;

	protected override void OnShot()
	{
		GameManager.Instance.LevelController.RotateLevel(clockWise);
	}
}
