using UnityEngine;

public abstract class UICoreViewBase : MonoBehaviour
{
	[SerializeField] private GameObject root;

	public void SetVisible(bool visible) => root.SetActive(visible);
}
