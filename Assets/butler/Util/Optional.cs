using System;
using UnityEngine;

[Serializable]
public abstract class Optional
{
	[SerializeField] protected bool hasValue;
	
	public bool HasValue => hasValue;
}

[Serializable]
public class Optional<T> : Optional
{
	[SerializeField] private T value;

	public T Value => hasValue ? value : throw new InvalidOperationException();

	public bool TryGet(out T v)
	{
		v = hasValue ? value : default;
		return hasValue;
	}
}
