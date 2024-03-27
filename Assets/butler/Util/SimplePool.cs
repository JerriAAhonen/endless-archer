using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SimplePool<T> where T : Component
{
	private readonly IObjectPool<T> pool;

	// https://docs.unity3d.com/ScriptReference/Pool.ObjectPool_1.html

	public SimplePool(T poolObject, Transform parent, MonoBehaviour loader, Action poolInitialized, bool preCoock = true, int defaultCapacity = 10, int maxSize = 30)
	{
		pool = new ObjectPool<T>(
			() => UnityEngine.Object.Instantiate(poolObject, parent),
			o => o.gameObject.SetActive(true),
			o => o.gameObject.SetActive(false),
			UnityEngine.Object.Destroy,
			true,
			defaultCapacity,
			maxSize);

		if (!preCoock)
		{
			poolInitialized?.Invoke();
			return;
		}

		loader.StartCoroutine(InitializationRoutine());

		IEnumerator InitializationRoutine()
		{
			var preCoockedObjects = new T[defaultCapacity];
			for (int i = 0; i < defaultCapacity; i++)
			{
				preCoockedObjects[i] = pool.Get();
				yield return null;
			}

			for (int i = 0; i < defaultCapacity; i++)
				pool.Release(preCoockedObjects[i]);

			poolInitialized?.Invoke();
		}
	}

	public SimplePool(List<T> poolObjects, Transform parent, MonoBehaviour loader, Action poolInitialized, bool preCoock = true, int defaultCapacity = 10, int maxSize = 30)
	{
		pool = new ObjectPool<T>(
			() => 
			{
				var rand = poolObjects.Random();
				if (rand == null)
					Debug.LogError("List of pool objects contains a null object!");
				return UnityEngine.Object.Instantiate(rand, parent);
			},
			o => o.gameObject.SetActive(true),
			o => o.gameObject.SetActive(false),
			UnityEngine.Object.Destroy,
			true,
			defaultCapacity,
			maxSize);

		if (!preCoock)
		{
			poolInitialized?.Invoke();
			return;
		}

		loader.StartCoroutine(InitializationRoutine());

		IEnumerator InitializationRoutine()
		{
			var preCoockedObjects = new T[defaultCapacity];
			for (int i = 0; i < defaultCapacity; i++)
			{
				preCoockedObjects[i] = pool.Get();
				yield return null;
			}

			for (int i = 0; i < defaultCapacity; i++)
				pool.Release(preCoockedObjects[i]);

			poolInitialized?.Invoke();
		}
	}

	public T Get() => pool.Get();
	public void Release(T o) => pool.Release(o);
}
