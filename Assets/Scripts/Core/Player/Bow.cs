using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
	[SerializeField] private float drawSpeed;
	[SerializeField] private float maxDraw = 1f;
	[SerializeField] private float drawVisualMultiplier = 1f;

	[SerializeField] private Transform arrowContainer;
	[SerializeField] private Arrow arrowPrefab;

	private PlayerController controller;
	private Arrow arrow;
	private float draw;

	public void Init(PlayerController controller)
	{
		this.controller = controller;
		SpawnArrow();
	}

	private void Update()
	{
		if (controller == null) return;
		if (!controller.ControlsEnabled) return;

		if (Input.GetMouseButton(0))
		{
			draw += Time.deltaTime * drawSpeed;
			draw = Mathf.Min(draw, maxDraw);
			MoveArrowWithDraw();
		}
		if (Input.GetMouseButtonUp(0))
		{
			Shoot();
			draw = 0f;
		}
	}

	private void Shoot()
	{
		StartCoroutine(ShootRoutine());

		IEnumerator ShootRoutine()
		{
			arrow.Shoot(draw);

			yield return CachedWait.ForSeconds(0.5f);

			SpawnArrow();
		}
	}

	private void SpawnArrow()
	{
		arrow = Instantiate(arrowPrefab, arrowContainer);
		arrow.transform.localPosition = Vector3.zero;
		arrow.transform.localRotation = Quaternion.identity;
	}

	private void MoveArrowWithDraw()
	{
		if (!arrow) return;

		arrow.transform.localPosition = Vector3.back * draw * drawVisualMultiplier;
	}
}
