using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public static class Extensions
{
	#region List

	public static T Random<T>(this List<T> self)
	{
		if (self.Count == 0)
			return default;
		return self[UnityEngine.Random.Range(0, self.Count)];
	}

	public static int RemoveDuplicates<T>(this List<T> self)
	{
		var set = new HashSet<T>(self.Count);
		int removed = 0;

		for (int i = 0; i < self.Count; i++)
		{
			if (!set.Add(self[i]))
			{
				self.RemoveAt(i);
				i--;
				removed++;
			}
		}

		return removed;
	}

	/// <summary>
	/// Determines whether a collection is null or has no elements
	/// without having to enumerate the entire collection to get a count.
	///
	/// Uses LINQ's Any() method to determine if the collection is empty,
	/// so there is some GC overhead.
	/// </summary>
	/// <param name="list">List to evaluate</param>
	public static bool IsNullOrEmpty<T>(this IList<T> list)
	{
		return list == null || !list.Any();
	}

	/// <summary>
	/// Creates a new list that is a copy of the original list.
	/// </summary>
	/// <param name="list">The original list to be copied.</param>
	/// <returns>A new list that is a copy of the original list.</returns>
	public static List<T> Clone<T>(this IList<T> list)
	{
		List<T> newList = new List<T>();
		foreach (T item in list)
		{
			newList.Add(item);
		}

		return newList;
	}

	/// <summary>
	/// Swaps two elements in the list at the specified indices.
	/// </summary>
	/// <param name="list">The list.</param>
	/// <param name="indexA">The index of the first element.</param>
	/// <param name="indexB">The index of the second element.</param>
	public static void Swap<T>(this IList<T> list, int indexA, int indexB)
	{
		(list[indexA], list[indexB]) = (list[indexB], list[indexA]);
	}

	#endregion
	#region IReadOnlyList

	public static T Find<T>(this IReadOnlyList<T> self, Predicate<T> predicate)
	{
		foreach (var t in self)
			if (predicate(t))
				return t;
		return default;
	}

	public static bool TryFind<T>(this IReadOnlyList<T> self, out T result, Predicate<T> predicate)
	{
		foreach (var t in self)
		{
			if (predicate(t))
			{
				result = t;
				return true;
			}
		}

		result = default;
		return false;
	}

	#endregion
	#region Vector3

	public static bool Approximately(this Vector3 self, Vector3 other)
	{
		var x = Mathf.Approximately(self.x, other.x);
		var y = Mathf.Approximately(self.y, other.y);
		var z = Mathf.Approximately(self.z, other.z);
		return x && y && z;
	}

	/// <summary>
	/// Clamp every value between min and max
	/// </summary>
	/// <param name="self"></param>
	/// <param name="min">Minimum value for all 3 axis</param>
	/// <param name="max">Maximum value for all 3 axis</param>
	/// <returns></returns>
	public static Vector3 Clamp(this Vector3 self, float min, float max)
	{
		self.x = Mathf.Clamp(self.x, min, max);
		self.y = Mathf.Clamp(self.y, min, max);
		self.z = Mathf.Clamp(self.z, min, max);
		return self;
	}

	public static Vector3 SetY(this Vector3 self, float y)
	{
		self.y = y;
		return self;
	}

	public static Vector3Int ToVector3Int(this Vector3 v)
	{
		return new Vector3Int((int)v.x, (int)v.y, (int)v.z);
	}

	/// <summary>
	/// Sets any x y z values of a Vector3
	/// </summary>
	public static Vector3 With(this Vector3 vector, float? x = null, float? y = null, float? z = null)
	{
		return new Vector3(x ?? vector.x, y ?? vector.y, z ?? vector.z);
	}

	/// <summary>
	/// Adds to any x y z values of a Vector3
	/// </summary>
	public static Vector3 Add(this Vector3 vector, float x = 0, float y = 0, float z = 0)
	{
		return new Vector3(vector.x + x, vector.y + y, vector.z + z);
	}

	/// <summary>
	/// Returns a Boolean indicating whether the current Vector3 is in a given range from another Vector3
	/// </summary>
	/// <param name="current">The current Vector3 position</param>
	/// <param name="target">The Vector3 position to compare against</param>
	/// <param name="range">The range value to compare against</param>
	/// <returns>True if the current Vector3 is in the given range from the target Vector3, false otherwise</returns>
	public static bool InRangeOf(this Vector3 current, Vector3 target, float range)
	{
		return (current - target).sqrMagnitude <= range * range;
	}

	#endregion
	#region Vector2

	/// <summary>
	/// Adds to any x y values of a Vector2
	/// </summary>
	public static Vector2 Add(this Vector2 vector2, float x = 0, float y = 0)
	{
		return new Vector2(vector2.x + x, vector2.y + y);
	}

	/// <summary>
	/// Sets any x y values of a Vector2
	/// </summary>
	public static Vector2 With(this Vector2 vector2, float? x = null, float? y = null)
	{
		return new Vector2(x ?? vector2.x, y ?? vector2.y);
	}

	/// <summary>
	/// Returns a Boolean indicating whether the current Vector2 is in a given range from another Vector2
	/// </summary>
	/// <param name="current">The current Vector2 position</param>
	/// <param name="target">The Vector2 position to compare against</param>
	/// <param name="range">The range value to compare against</param>
	/// <returns>True if the current Vector2 is in the given range from the target Vector2, false otherwise</returns>
	public static bool InRangeOf(this Vector2 current, Vector2 target, float range)
	{
		return (current - target).sqrMagnitude <= range * range;
	}

	#endregion
	#region GameObject

	public static T GetOrAddComponent<T>(this GameObject go) where T : Component
	{
		return go.GetComponent<T>() ?? go.AddComponent<T>();
	}

	/// <summary>
	/// This method is used to hide the GameObject in the Hierarchy view.
	/// </summary>
	/// <param name="gameObject"></param>
	public static void HideInHierarchy(this GameObject gameObject)
	{
		gameObject.hideFlags = HideFlags.HideInHierarchy;
	}

	/// <summary>
	/// Recursively sets the provided layer for this GameObject and all of its descendants in the Unity scene hierarchy.
	/// </summary>
	/// <param name="gameObject">The GameObject to set layers for.</param>
	/// <param name="layer">The layer number to set for GameObject and all of its descendants.</param>
	public static void SetLayersRecursively(this GameObject gameObject, int layer)
	{
		gameObject.layer = layer;
		gameObject.transform.ForEveryChild(child => child.gameObject.SetLayersRecursively(layer));
	}

	/// <summary>
	/// Returns the object itself if it exists, null otherwise.
	/// </summary>
	/// <remarks>
	/// This method helps differentiate between a null reference and a destroyed Unity object. Unity's "== null" check
	/// can incorrectly return true for destroyed objects, leading to misleading behaviour. The OrNull method use
	/// Unity's "null check", and if the object has been marked for destruction, it ensures an actual null reference is returned,
	/// aiding in correctly chaining operations and preventing NullReferenceExceptions.
	/// </remarks>
	/// <typeparam name="T">The type of the object.</typeparam>
	/// <param name="obj">The object being checked.</param>
	/// <returns>The object itself if it exists and not destroyed, null otherwise.</returns>
	public static T OrNull<T>(this T obj) where T : Object => obj ? obj : null;

	/// <summary>
	/// Destroys all children of the game object
	/// </summary>
	/// <param name="gameObject">GameObject whose children are to be destroyed.</param>
	public static void DestroyChildren(this GameObject gameObject)
	{
		gameObject.transform.DestroyChildren();
	}

	/// <summary>
	/// Immediately destroys all children of the given GameObject.
	/// </summary>
	/// <param name="gameObject">GameObject whose children are to be destroyed.</param>
	public static void DestroyChildrenImmediate(this GameObject gameObject)
	{
		gameObject.transform.DestroyChildrenImmediate();
	}

	/// <summary>
	/// Enables all child GameObjects associated with the given GameObject.
	/// </summary>
	/// <param name="gameObject">GameObject whose child GameObjects are to be enabled.</param>
	public static void EnableChildren(this GameObject gameObject)
	{
		gameObject.transform.EnableChildren();
	}

	/// <summary>
	/// Disables all child GameObjects associated with the given GameObject.
	/// </summary>
	/// <param name="gameObject">GameObject whose child GameObjects are to be disabled.</param>
	public static void DisableChildren(this GameObject gameObject)
	{
		gameObject.transform.DisableChildren();
	}

	/// <summary>
	/// Resets the GameObject's transform's position, rotation, and scale to their default values.
	/// </summary>
	/// <param name="gameObject">GameObject whose transformation is to be reset.</param>
	public static void ResetTransformation(this GameObject gameObject)
	{
		gameObject.transform.Reset();
	}

	/// <summary>
	/// Returns the hierarchical path in the Unity scene hierarchy for this GameObject.
	/// </summary>
	/// <param name="gameObject">The GameObject to get the path for.</param>
	/// <returns>A string representing the full hierarchical path of this GameObject in the Unity scene.
	/// This is a '/'-separated string where each part is the name of a parent, starting from the root parent and ending
	/// with the name of the specified GameObjects parent.</returns>
	public static string Path(this GameObject gameObject)
	{
		return "/" + string.Join("/", gameObject.GetComponentsInParent<Transform>().Select(t => t.name).Reverse().ToArray());
	}

	/// <summary>
	/// Returns the full hierarchical path in the Unity scene hierarchy for this GameObject.
	/// </summary>
	/// <param name="gameObject">The GameObject to get the path for.</param>
	/// <returns>A string representing the full hierarchical path of this GameObject in the Unity scene.
	/// This is a '/'-separated string where each part is the name of a parent, starting from the root parent and ending
	/// with the name of the specified GameObject itself.</returns>
	public static string PathFull(this GameObject gameObject)
	{
		return gameObject.Path() + "/" + gameObject.name;
	}

	#endregion
	#region Transform

	/// <summary>
	/// Retrieves all the children of a given Transform.
	/// </summary>
	/// <remarks>
	/// This method can be used with LINQ to perform operations on all child Transforms. For example,
	/// you could use it to find all children with a specific tag, to disable all children, etc.
	/// Transform implements IEnumerable and the GetEnumerator method which returns an IEnumerator of all its children.
	/// </remarks>
	/// <param name="parent">The Transform to retrieve children from.</param>
	/// <returns>An IEnumerable&lt;Transform&gt; containing all the child Transforms of the parent.</returns>    
	public static IEnumerable<Transform> Children(this Transform parent)
	{
		foreach (Transform child in parent)
		{
			yield return child;
		}
	}

	/// <summary>
	/// Resets transform's position, scale and rotation
	/// </summary>
	/// <param name="transform">Transform to use</param>
	public static void Reset(this Transform transform)
	{
		transform.position = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
	}

	/// <summary>
	/// Destroys all child game objects of the given transform.
	/// </summary>
	/// <param name="parent">The Transform whose child game objects are to be destroyed.</param>
	public static void DestroyChildren(this Transform parent)
	{
		parent.ForEveryChild(child => Object.Destroy(child.gameObject));
	}

	/// <summary>
	/// Immediately destroys all child game objects of the given transform.
	/// </summary>
	/// <param name="parent">The Transform whose child game objects are to be immediately destroyed.</param>
	public static void DestroyChildrenImmediate(this Transform parent)
	{
		parent.ForEveryChild(child => Object.DestroyImmediate(child.gameObject));
	}

	/// <summary>
	/// Enables all child game objects of the given transform.
	/// </summary>
	/// <param name="parent">The Transform whose child game objects are to be enabled.</param>
	public static void EnableChildren(this Transform parent)
	{
		parent.ForEveryChild(child => child.gameObject.SetActive(true));
	}

	/// <summary>
	/// Disables all child game objects of the given transform.
	/// </summary>
	/// <param name="parent">The Transform whose child game objects are to be disabled.</param>
	public static void DisableChildren(this Transform parent)
	{
		parent.ForEveryChild(child => child.gameObject.SetActive(false));
	}

	/// <summary>
	/// Executes a specified action for each child of a given transform.
	/// </summary>
	/// <param name="parent">The parent transform.</param>
	/// <param name="action">The action to be performed on each child.</param>
	/// <remarks>
	/// This method iterates over all child transforms in reverse order and executes a given action on them.
	/// The action is a delegate that takes a Transform as parameter.
	/// </remarks>
	public static void ForEveryChild(this Transform parent, System.Action<Transform> action)
	{
		for (var i = parent.childCount - 1; i >= 0; i--)
		{
			action(parent.GetChild(i));
		}
	}

	#endregion
	#region CanvasGroup

	public static void Toggle(this CanvasGroup self)
	{
		SetVisible(self, Mathf.Approximately(self.alpha, 0f));
	}

	public static void SetVisible(this CanvasGroup self, bool visible)
	{
		self.alpha = visible ? 1f : 0f;
		self.interactable = visible;
		self.blocksRaycasts = visible;
	}

	#endregion
	#region Color

	public static Color Opacity(this Color self, float opacity)
	{
		self.a = opacity;
		return self;
	}

	#endregion
	#region Image

	public static void SetAlpha(this Image self, float alpha)
	{
		var color = self.color;
		color.a = alpha;
		self.color = color;
	}

	#endregion
	#region Numbers

	public static float PercentageOf(this int part, int whole)
	{
		if (whole == 0) return 0; // Handling division by zero
		return (float)part / whole;
	}

	public static int AtLeast(this int value, int min) => Mathf.Max(value, min);
	public static int AtMost(this int value, int max) => Mathf.Min(value, max);

	public static float AtLeast(this float value, float min) => Mathf.Max(value, min);
	public static float AtMost(this float value, float max) => Mathf.Min(value, max);

	public static bool Approximately(this float value, float compareTo) =>  Mathf.Approximately(value, compareTo);

	#endregion
	#region String

	public static bool IsNullOrEmpty(this string self)
	{
		return self == null || self.Length == 0;
	}

	public static string ToCustomString(this int number,
		int position = 3, string separator = " ")
	{
		var numStr = number.ToString();
		var len = numStr.Length;
		var result = new StringBuilder();
		for (var i = 0; i < len; i++)
		{
			if (i > 0 && i % position == 0) result.Insert(0, separator);
			result.Insert(0, numStr[len - 1 - i]);
		}
		return result.ToString();
	}

	#endregion
}
