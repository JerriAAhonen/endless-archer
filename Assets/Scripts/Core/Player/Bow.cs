using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Bow : MonoBehaviour
{
	[SerializeField] private GameObject root;
	[Header("Draw")]
	[SerializeField] private float maxDrawDur;
	[SerializeField] private AnimationCurve drawSpeedCurve;
	[SerializeField] private float drawVisualMultiplier = 1f;
	[SerializeField] private float arrowOffset = 0.9f;
	[Header("Release")]
	[SerializeField] private float releaseDur = 0.1f;
	[SerializeField] private float newArrowDelay = 0.2f;
	[Header("Arrow")]
	[SerializeField] private Transform arrowContainer;
	[SerializeField] private Arrow arrowPrefab;
	[SerializeField] private Vector3 arrowSpawnRotation;
	[Header("Position")]
	[SerializeField] private Transform positionRef;
	[SerializeField] private float movementSpeed;
	[Header("Animations")]
	[SerializeField] private SkinnedMeshRenderer bowSMR;
	[SerializeField] private SkinnedMeshRenderer stringSMR;
	[Header("Sounds")]
	[SerializeField] private AudioEvent load;
	[SerializeField] private AudioEvent shoot;

	private IObjectPool<Arrow> arrowPool;
	private PlayerController controller;
	private Arrow currentArrow;
	private float elapsedDraw;
	private float drawAmount;

	public void Init(PlayerController controller)
	{
		this.controller = controller;
	}

	private void Awake()
	{
		root.SetActive(false);

		arrowPool = new ObjectPool<Arrow>(
			CreateArrow,
			a => InitArrow(a),
			a => ReleaseArrow(a),
			a => Destroy(a.gameObject),
			false);

		Arrow CreateArrow()
		{
			var arrow = Instantiate(arrowPrefab, arrowContainer);
			InitArrow(arrow);
			return arrow;
		}

		void InitArrow(Arrow arrow)
		{
			arrow.Init(a => arrowPool.Release(a));
			arrow.transform.parent = arrowContainer;
			arrow.transform.localPosition = Vector3.forward * arrowOffset;
			arrow.transform.localRotation = Quaternion.Euler(arrowSpawnRotation);
			arrow.gameObject.SetActive(true);
		}

		void ReleaseArrow(Arrow arrow)
		{
			arrow.transform.parent = null;
			arrow.gameObject.SetActive(false);
		}
	}

	private void Update()
	{
		if (controller == null) return;
		if (controller.BlockInput) return;
		if (!controller.ControlsEnabled) return;
		if (!currentArrow) return;

		// TODO Switch to input manager events
		if (Input.GetMouseButtonDown(0))
		{
			AudioManager.Instance.PlayOnce(load);
		}
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
		// Smoothly follow the ref position and rotation
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
		currentArrow = null;
		ResetBow();

		Debug.Assert(arrowContainer.childCount == 0, "Arrow container has an arrow left in it");
	}

	private void SpawnArrow()
	{
		Debug.Assert(arrowContainer.childCount == 0, "Arrow container already has an arrow in it");

		currentArrow = arrowPool.Get();
	}

	private void MoveArrowWithDraw(float percentage)
	{
		currentArrow.transform.localPosition = (Vector3.back * (percentage * drawVisualMultiplier) + (Vector3.forward * arrowOffset));
	}

	private void AnimateBow(float percentage)
	{
		bowSMR.SetBlendShapeWeight(0, percentage * 100f);
		stringSMR.SetBlendShapeWeight(0, percentage * 100f);
	}

	private void Shoot(float percentage)
	{
		StartCoroutine(ShootRoutine());

		IEnumerator ShootRoutine()
		{
			AudioManager.Instance.PlayOnce(shoot);
			AnimateBowRelease();
			currentArrow.Shoot(percentage);
			currentArrow = null;

			yield return CachedWait.ForSeconds(newArrowDelay);

			SpawnArrow();
		}
	}

	private void AnimateBowRelease()
	{
		var weight = bowSMR.GetBlendShapeWeight(0);
		StartCoroutine(ReleaseRoutine(bowSMR, weight));

		weight = stringSMR.GetBlendShapeWeight(0);
		StartCoroutine(ReleaseRoutine(stringSMR, weight));

		IEnumerator ReleaseRoutine(SkinnedMeshRenderer smr, float from)
		{
			var elapsed = 0f;
			while (elapsed < releaseDur)
			{
				elapsed += Time.deltaTime;
				yield return null;

				var percentage = elapsed / releaseDur;

				smr.SetBlendShapeWeight(0, from - (from * percentage));
			}

			smr.SetBlendShapeWeight(0, 0f);
		}
	}

	private void ResetBow()
	{
		bowSMR.SetBlendShapeWeight(0, 0f);
		stringSMR.SetBlendShapeWeight(0, 0f);
	}
}
