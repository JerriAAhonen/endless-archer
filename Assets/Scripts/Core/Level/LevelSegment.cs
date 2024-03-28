using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSegment : MonoBehaviour
{
    private float despawnDist;
    private Action<LevelSegment> onDespawnDistReached;

    public void Init(float despawnDist, Action<LevelSegment> onDespawnDistReached)
    {
        this.despawnDist = despawnDist;
        this.onDespawnDistReached = onDespawnDistReached;
    }

    public void Translate(float dist)
    {
        transform.Translate(0, 0, dist * Time.deltaTime, Space.World);

        if (transform.localPosition.z < despawnDist)
            onDespawnDistReached(this);
    }
}
