using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingTarget : MonoBehaviour
{
	[SerializeField] private LayerMask arrowMask;
	[SerializeField] private float labelOffset;
	[Header("Points")]
	[SerializeField] private int pointValue;
	[SerializeField] private string text;
	[SerializeField] private Color color;

	private void OnCollisionEnter(Collision collision)
	{
		if (BitMaskUtil.MaskContainsLayer(arrowMask, collision.gameObject.layer))
		{
			GameManager.Instance.LevelController.FloatingText.ShowText(transform.position + Vector3.up * labelOffset, text, color);
			GameManager.Instance.LevelController.Score.Add(pointValue);
			Destroy(gameObject);
		}
	}
}
