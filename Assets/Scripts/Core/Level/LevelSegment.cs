using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSegment : MonoBehaviour
{
    [SerializeField] private LevelSegmentType type;
    [SerializeField] private List<ShootingTargetBase> shootingTargets;

    private float despawnDist;
    private Action<LevelSegment> onDespawnDistReached;

    public LevelSegmentType Type => type;

    public void Init(float despawnDist, Action<LevelSegment> onDespawnDistReached)
    {
        this.despawnDist = despawnDist;
        this.onDespawnDistReached = onDespawnDistReached;
    }

    public void Activate(bool activate)
    {
        gameObject.SetActive(activate);
        
        if (activate)
        {
			foreach (var target in shootingTargets)
			{
				target.SetActive(true);
			}
		}
    }

    public void Translate(float dist)
    {
        transform.Translate(0, 0, dist * Time.deltaTime, Space.World);

        if (transform.localPosition.z < despawnDist)
            onDespawnDistReached(this);
    }
}
