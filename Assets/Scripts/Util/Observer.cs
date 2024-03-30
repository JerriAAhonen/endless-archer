using System;
using UnityEngine;

public class Observer<T>
{
	[SerializeField] private T value;

	private event Action<T> ValueChanged;

	public T Value
	{
		get => value;
		set => Set(value);
	}

	public static implicit operator T(Observer<T> observer) => observer.value;

	public Observer(T value, Action<T> onValueChanged = null)
	{
		this.value = value;
		AddListener(onValueChanged);
	}

	public void AddListener(Action<T> onValueChanged)
	{
		if (onValueChanged != null)
			ValueChanged += onValueChanged;

		ValueChanged?.Invoke(value);
	}

	private void Set(T value)
	{
		if (Equals(this.value, value))
			return;

		this.value = value;
		ValueChanged?.Invoke(value);
	}

}
