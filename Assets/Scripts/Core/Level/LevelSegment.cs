using System;
using System.Collections.Generic;
using NaughtyAttributes;
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

        if (!activate)
            return;

        transform.localRotation = Quaternion.Euler(GetRandomRotation());
		foreach (var target in shootingTargets)
		{
			target.SetActive(true);
		}
    }

    public void Translate(float dist)
    {
        transform.Translate(0, 0, dist * Time.deltaTime, Space.World);

        if (transform.localPosition.z < despawnDist)
            Despawn();
	}

    private void Despawn()
    {
        // Check for any arrows and release them back to pool
        foreach (var arrow in GetComponentsInChildren<Arrow>())
            arrow.ReleaseToPool();

		onDespawnDistReached?.Invoke(this);
	}

    private Vector3 GetRandomRotation()
    {
        var rand = UnityEngine.Random.Range(0, 4);
        return rand switch
        {
            0 => new Vector3(0, 0, 0),
            1 => new Vector3(0, 0, 90),
            2 => new Vector3(0, 0, 180),
            3 => new Vector3(0, 0, 270),
            _ => throw new NotImplementedException(),
        };
	}

    [Button("Refresh Targets")]
#pragma warning disable IDE0051 // Remove unused private members
	private void EDITOR_GetTargets()
#pragma warning restore IDE0051 // Remove unused private members
	{
		shootingTargets.Clear();
        shootingTargets.AddRange(GetComponentsInChildren<ShootingTargetBase>());
    }
}
