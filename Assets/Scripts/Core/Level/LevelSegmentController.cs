using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSegmentController : MonoBehaviour
{
	[SerializeField] private LevelSegmentDatabase segmentDB;
	[SerializeField] private Transform segmentContainer;
	[SerializeField] private float segmentAmount;
	[SerializeField] private float segmentLength;
	[Header("Difficulty")]
	[SerializeField] private float baseMovementSpeed; // How fast do the segments move?
	[Space]
	[SerializeField] private float initialSpeedUpDur; // How long does the initial windup take?
	[SerializeField] private float initialSpeedUpFinalSpeed; // How fast should we be going once the initial windup is complete?
	[Space]
	[SerializeField] private float movementSpeedIncrease; // How fast should we be speeding up after the initial windup?

	private readonly List<LevelSegment> activeSegments = new();
	private readonly Dictionary<LevelSegmentType, List<LevelSegment>> segmentInstances = new();

	private LevelController controller;
	private float movementSpeed;

	// Rotation
	private int? rotateTweenId;
	private Vector3 targetRot;

	private float DespawnDistance => segmentContainer.position.z - segmentLength * 2;

	public void Init(LevelController controller)
	{
		this.controller = controller;
		movementSpeed = baseMovementSpeed;

		for (int i = 0; i < segmentAmount; i++)
		{
			ActivateSegment(LevelSegmentType.Default);
		}
	}

	public void OnStartLevel()
	{
		movementSpeed = baseMovementSpeed;
		StopAllCoroutines();
		StartCoroutine(ResetRoutine());
		StartCoroutine(InitialSpeedUpRoutine());
	}

	public void OnGameOver()
	{
		movementSpeed = baseMovementSpeed;
		StopAllCoroutines();
		StartCoroutine(ResetRoutine());
	}

	public void Rotate(bool clockwise)
	{
		if (rotateTweenId.HasValue)
			LeanTween.cancel(rotateTweenId.Value);

		targetRot.z += clockwise ? -90 : 90;

		rotateTweenId = LeanTween.rotateLocal(segmentContainer.gameObject, targetRot, 0.5f)
			.setOnComplete(() => rotateTweenId = null)
			.setEaseOutBack()
			.uniqueId;
	}

	private IEnumerator ResetRoutine()
	{
		for (int i = 0; i < segmentAmount; i++)
		{
			var segment = activeSegments.Last();
			segment.Activate(false);
			activeSegments.Remove(segment);
			segmentInstances[segment.Type].Add(segment);
			yield return null;
			yield return null;
			yield return null;
		}

		for (int i = 0; i < segmentAmount; i++)
		{
			ActivateSegment(LevelSegmentType.Default);
			yield return null;
			yield return null;
			yield return null;
		}
	}

	private IEnumerator InitialSpeedUpRoutine()
	{
		var diff = initialSpeedUpFinalSpeed - baseMovementSpeed;
		var step = diff / initialSpeedUpDur;

		var elapsed = 0f;
		while (elapsed < initialSpeedUpDur)
		{
			elapsed += Time.deltaTime;
			movementSpeed += step * Time.deltaTime;
			yield return null;
		}
	}

	private void Update()
	{
		for (int i = 0; i < activeSegments.Count; i++)
		{
			activeSegments[i].Translate(-movementSpeed); // Inverse movementSpeed to make segments move backwards by default
		}

		if (controller.State == LevelState.Idle)
			return;

		movementSpeed += movementSpeedIncrease * Time.deltaTime;
	}

	private void OnSegmentReachedDespawnDistance(LevelSegment segment)
	{
		segment.Activate(false);

		activeSegments.Remove(segment);
		segmentInstances[segment.Type].Add(segment);

		// When game over / in mainmenu, spawn only default pieces
		if (controller.State == LevelState.Idle)
		{
			ActivateSegment(LevelSegmentType.Default);
			return;
		}

		var lastSegmentType = activeSegments[^1].Type;
		var filter = LevelSegmentTypeUtil.GetFilterForNextType(lastSegmentType);
		var newSegmentType = EnumUtils.GetRandom(filter);
		ActivateSegment(newSegmentType);
	}

	private void ActivateSegment(LevelSegmentType type)
	{
		if (!segmentInstances.ContainsKey(type))
			segmentInstances[type] = new();

		if (segmentInstances[type].Count == 0)
		{
			SpawnNewSegment(type); 
			return;
		}

		var newSegment = segmentInstances[type][0];
		segmentInstances[type].RemoveAt(0);

		newSegment.Activate(true);
		newSegment.transform.localPosition = GetSpawnPos();
		activeSegments.Add(newSegment);
	}

	private void SpawnNewSegment(LevelSegmentType type)
	{
		var segment = Instantiate(segmentDB.Get(type), segmentContainer);
		segment.Init(DespawnDistance, OnSegmentReachedDespawnDistance);
		segment.Activate(true);
		segment.transform.localPosition = GetSpawnPos();
		activeSegments.Add(segment);

		if (!segmentInstances.ContainsKey(type))
			segmentInstances[type] = new();
	}

	private Vector3 GetSpawnPos()
	{
		if (activeSegments == null || activeSegments.Count == 0)
			return Vector3.zero;
		return activeSegments.Last().transform.localPosition + Vector3.forward * segmentLength;
	}
}
