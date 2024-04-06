using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class UIButtonAudio : MonoBehaviour
{
	[SerializeField] private bool overrideSFX;
	[ShowIf("overrideSFX")]
	[SerializeField] private AudioEvent overrideClickSFX;

	private Button button;

	private void Awake()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
	}

	private void OnClick()
	{
		if (overrideSFX && overrideClickSFX)
			AudioManager.Instance.PlayOnce(overrideClickSFX);
		else
			AudioManager.Instance.PlayDefaultButtonClick();
	}
}
