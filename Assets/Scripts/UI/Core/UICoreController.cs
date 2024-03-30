using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICoreController : MonoBehaviour
{
	[SerializeField] private GameObject root;
	[SerializeField] private UIScoreView score;

	public UIScoreView Score => score;

	public void SetVisible(bool visible) => root.SetActive(visible);
}
