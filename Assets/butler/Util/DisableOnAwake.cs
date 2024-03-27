using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableOnAwake : MonoBehaviour
{
	[SerializeField] private Behaviour[] toDisable;

	private void Awake()
	{
		foreach (var b in toDisable)
			if (Is.NotNull(b))
				b.enabled = false;
	}
}
