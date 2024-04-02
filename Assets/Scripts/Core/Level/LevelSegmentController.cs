using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum LevelSegmentStyle
{
	Level,
	Menu
}

public class LevelSegmentController : MonoBehaviour
{
	[SerializeField] private LevelSegmentDatabase segmentDB;
	[SerializeField] private Transform segmentContainer;
	[SerializeField] private float segmentAmount;
	[SerializeField] private float segmentLength;
	[SerializeField] private float movementSpeed; // How fast do the segments move?

	private readonly List<LevelSegment> activeSegments = new();

	private readonly Dictionary<LevelSegmentType, List<LevelSegment>> segmentInstances = new();

	//private LevelSegmentStyle currentStyle;
	private float DespawnDistance => segmentContainer.position.z - segmentLength * 2;

	public void StartLevel()
	{
		//currentStyle = LevelSegmentStyle.Level;
		PopulateSegments();
	}

	public void SetMenu()
	{
		//currentStyle = LevelSegmentStyle.Menu;
	}

	private void Update()
	{
		#region debug

		if (Input.GetKeyDown(KeyCode.Q))
		{
			// Rotate counterclock-wise
			LeanTween.rotateAround(segmentContainer.gameObject, Vector3.forward, 90, 0.5f).setEaseOutBack();
		}

		if (Input.GetKeyDown(KeyCode.E))
		{
			// Rotate clock-wise
			LeanTween.rotateAround(segmentContainer.gameObject, Vector3.forward, -90, 0.5f).setEaseOutBack();
		}

		#endregion

		for (int i = 0; i < activeSegments.Count; i++)
		{
			activeSegments[i].Translate(-movementSpeed); // Inverse movementSpeed to make segments move backwards by default
		}
	}

	private void OnSegmentReachedDespawnDistance(LevelSegment segment)
	{
		segment.Activate(false);

		activeSegments.Remove(segment);
		segmentInstances[segment.Type].Add(segment);

		var newSegmentType = EnumUtils.GetRandom(new[] { LevelSegmentType.None });

		if (!segmentInstances.ContainsKey(newSegmentType))
			segmentInstances[newSegmentType] = new();

		if (segmentInstances[newSegmentType].Count == 0)
		{
			SpawnNewSegment(newSegmentType);
			return;
		}

		var newSegment = segmentInstances[newSegmentType][0];
		segmentInstances[newSegmentType].RemoveAt(0);

		newSegment.Activate(true);
		newSegment.transform.localPosition = GetSpawnPos();
		activeSegments.Add(newSegment);
	}

	private void PopulateSegments()
	{
		for (int i = 0; i < segmentAmount; i++)
		{
			SpawnNewSegment(LevelSegmentType.Default);
		}
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
