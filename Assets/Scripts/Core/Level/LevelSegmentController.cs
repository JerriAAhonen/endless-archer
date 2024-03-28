using System.Collections;
using System.Collections.Generic;
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

	private LevelSegmentStyle currentStyle;
	private float respawnDistance;
	private float despawnDistance;

	public void StartLevel()
	{
		currentStyle = LevelSegmentStyle.Level;
		PopulateSegments();
	}

	public void SetMenu()
	{
		currentStyle = LevelSegmentStyle.Menu;
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
			activeSegments[i].Translate(-movementSpeed); // Negate mvntSpd to make segments move backwards by default
		}
	}

	private void OnSegmentReachedDespawnDistance(LevelSegment segment)
	{
		segment.transform.position = transform.position + respawnDistance * Vector3.forward;

	}

	private void PopulateSegments()
	{
		despawnDistance = transform.position.z - segmentLength * 2;
		respawnDistance = despawnDistance + (segmentAmount - 1) * segmentLength;

		for (int i = 0; i < segmentAmount; i++)
		{
			var segment = Instantiate(segmentDB.Get(LevelSegmentType.Default), segmentContainer);
			segment.transform.position = i * segmentLength * Vector3.forward;
			segment.Init(despawnDistance, OnSegmentReachedDespawnDistance);
			activeSegments.Add(segment);
		}
	}
}
