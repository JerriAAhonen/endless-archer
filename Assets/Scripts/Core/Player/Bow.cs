using System.Collections;
using UnityEngine;

public class Bow : MonoBehaviour
{
	[SerializeField] private GameObject root;
	[Space]
	[SerializeField] private float maxDrawDur;
	[SerializeField] private AnimationCurve drawSpeedCurve;
	[SerializeField] private float drawVisualMultiplier = 1f;
	[SerializeField] private float arrowOffset = 0.9f;
	[Space]
	[SerializeField] private Transform arrowContainer;
	[SerializeField] private Arrow arrowPrefab;
	[SerializeField] private Vector3 arrowSpawnRotation;
	[Space]
	[SerializeField] private Transform positionRef;
	[SerializeField] private float movementSpeed;
	[Space]
	[SerializeField] private SkinnedMeshRenderer bowSMR;
	[SerializeField] private SkinnedMeshRenderer stringSMR;

	private PlayerController controller;
	private Arrow arrow;
	private float elapsedDraw;
	private float drawAmount;

	private void Awake()
	{
		root.SetActive(false);
	}

	public void Init(PlayerController controller)
	{
		this.controller = controller;
	}

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
			AnimateBow(drawAmount);
		}
		if (Input.GetMouseButtonUp(0))
		{
			Shoot(drawAmount);
			elapsedDraw = 0f;
			drawAmount = 0f;
		}
	}

	private void LateUpdate()
	{
		transform.position = Vector3.Lerp(transform.position, positionRef.position, movementSpeed * Time.deltaTime);
		transform.rotation = Quaternion.Lerp(transform.rotation, positionRef.rotation, movementSpeed * Time.deltaTime);
	}

	public void OnStartLevel()
	{
		root.SetActive(true);

		StartCoroutine(Routine());
		IEnumerator Routine()
		{
			yield return null;

			SpawnArrow();
		}
	}

	public void OnLevelEnded()
	{
		root.SetActive(false);

		if (arrow)
		{
			Destroy(arrow.gameObject);
			arrow = null;
		}
	}

	private void Shoot(float percentage)
	{
		if (!arrow) return;

		StartCoroutine(ShootRoutine());

		IEnumerator ShootRoutine()
		{
			AnimateBowRelease();
			arrow.Shoot(percentage);
			arrow = null;

			yield return CachedWait.ForSeconds(0.5f);

			SpawnArrow();
		}
	}

	private void SpawnArrow()
	{
		arrow = Instantiate(arrowPrefab, arrowContainer);
		arrow.transform.localPosition = Vector3.forward * arrowOffset;
		arrow.transform.localRotation = Quaternion.Euler(arrowSpawnRotation);
	}

	private void MoveArrowWithDraw(float percentage)
	{
		if (!arrow) return;

		arrow.transform.localPosition = (Vector3.back * (percentage * drawVisualMultiplier) + (Vector3.forward * arrowOffset));
	}

	private void AnimateBow(float percentage)
	{
		if (!arrow) return;

		bowSMR.SetBlendShapeWeight(0, percentage * 100f);
		stringSMR.SetBlendShapeWeight(0, percentage * 100f);
	}

	private void AnimateBowRelease()
	{
		var weight = bowSMR.GetBlendShapeWeight(0);
		StartCoroutine(ReleaseRoutine(bowSMR, weight));

		weight = stringSMR.GetBlendShapeWeight(0);
		StartCoroutine(ReleaseRoutine(stringSMR, weight));

		static IEnumerator ReleaseRoutine(SkinnedMeshRenderer smr, float from)
		{
			var dur = 0.2f;
			var elapsed = 0f;
			while (elapsed < dur)
			{
				elapsed += Time.deltaTime;
				yield return null;

				var percentage = elapsed / dur;

				smr.SetBlendShapeWeight(0, from - (from * percentage));
			}

			smr.SetBlendShapeWeight(0, 0f);
		}
	}
}
