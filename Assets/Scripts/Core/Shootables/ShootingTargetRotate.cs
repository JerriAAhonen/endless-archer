using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTargetRotate : ShootingTargetBase
{
	[Header("Rotation Direction")]
	[SerializeField] private bool clockWise;

	protected override void OnShot()
	{
		EventBus<Event_RotateLevel>.Raise(new Event_RotateLevel { clockwise = clockWise });
	}
}
