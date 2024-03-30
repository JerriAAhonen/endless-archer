using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : MonoBehaviour
{
	[SerializeField] private float maxDrawDur;
	[SerializeField] private AnimationCurve drawSpeedCurve;
	[SerializeField] private float drawVisualMultiplier = 1f;

	[SerializeField] private Transform arrowContainer;
	[SerializeField] private Arrow arrowPrefab;

	private PlayerController controller;
	private Arrow arrow;

	public void Init(PlayerController controller)
	{
		this.controller = controller;
	}

	private float elapsedDraw;
	private float drawAmount;

	private void Update()
	{
		if (controller == null) return;
		if (!controller.ControlsEnabled) return;

		if (Input.GetMouseButton(0))
		{
			elapsedDraw += Time.deltaTime;
			var drawDurPercentage = elapsedDraw / maxDrawDur;
			drawAmount = drawSpeedCurve.Evaluate(drawDurPercentage);
			MoveArrowWithDraw(drawAmount);
		}
		if (Input.GetMouseButtonUp(0))
		{
			Shoot(drawAmount);
			elapsedDraw = 0f;
			drawAmount = 0f;
		}
	}

	public void OnStartLevel()
	{
		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return null;

			SpawnArrow();
		}
	}

	public void OnLevelEnded()
	{
		Destroy(arrow.gameObject);
		arrow = null;
	}

	private void Shoot(float percentage)
	{
		if (!arrow) return;

		StartCoroutine(ShootRoutine());

		IEnumerator ShootRoutine()
		{
			arrow.Shoot(percentage);
			arrow = null;

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

	private void MoveArrowWithDraw(float percentage)
	{
		if (!arrow) return;

		arrow.transform.localPosition = Vector3.back * percentage * drawVisualMultiplier;
	}
}
