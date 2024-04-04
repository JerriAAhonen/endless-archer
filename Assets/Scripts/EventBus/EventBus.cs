using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus<T> where T : IEvent
{
	private static readonly HashSet<IEventBinding<T>> bindings = new();

	public static void Register(EventBinding<T> binding) => bindings.Add(binding);
	public static void Deregister(EventBinding<T> binding) => bindings.Remove(binding);

	public static void Raise(T @event)
	{
		foreach (var binding in bindings)
		{
			binding.OnEvent.Invoke(@event);
			binding.OnEventNoArgs.Invoke();
		}
	}

	/// <summary>
	/// Called from EventBusUtil
	/// </summary>
#pragma warning disable IDE0051 // Remove unused private members
	static void Clear()
#pragma warning restore IDE0051 // Remove unused private members
	{
		Debug.Log($"Clearing {typeof(T).Name} bindings");
		bindings.Clear();
	}
}