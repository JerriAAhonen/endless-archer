using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMainPage : UIMenuPageBase
{
	[Header("Buttons")]
	[SerializeField] private Button buttonPlay;
	[SerializeField] private Button buttonSettings;
	[SerializeField] private Button buttonQuit;
	[Header("Highscore")]
	[SerializeField] private GameObject highscore;
	[SerializeField] private TextMeshProUGUI highscoreAmount;

	private void Awake()
	{
		buttonPlay.onClick.AddListener(OnPlay);
		buttonSettings.onClick.AddListener(OnSettings);
		buttonQuit.onClick.AddListener(OnQuit);
	}

	#region Menu Page

	public override void Enter()
	{
		SetHighscore();
		HideAll();
		gameObject.SetActive(true);

		new Sequence.Builder()
			.AddElement(0, () => Show(buttonPlay.gameObject))
			.AddElement(100, () => Show(buttonSettings.gameObject))
			.AddElement(100, () => Show(buttonQuit.gameObject))
			.AddElement(100, () => Show(highscore))
			.AddElement(100, () => Show(highscoreAmount.gameObject))
			.Start();

		void HideAll()
		{
			buttonPlay.transform.localScale = Vector3.zero;
			buttonSettings.transform.localScale = Vector3.zero;
			buttonQuit.transform.localScale = Vector3.zero;
			highscore.transform.localScale = Vector3.zero;
			highscoreAmount.transform.localScale = Vector3.zero;
		}

		void Show(GameObject go)
		{
			UIAnimUtil.AnimateScale(go, Vector3.one, LeanTweenType.easeOutBack, 0.1f);
		}
	}

	public override void Exit()
	{
		gameObject.SetActive(false);
	}

	#endregion

	private void OnPlay()
	{
		GameManager.Instance.StartLevel();
	}

	private void OnSettings()
	{
		controller.SwitchPage(MenuPageType.Settings);
	}

	private void OnQuit()
	{
		GameManager.Instance.QuitGame();
	}

	private void SetHighscore()
	{
		var showHighscore = PlayerPrefsUtil.Highscore > 0;
		highscore.SetActive(showHighscore);
		highscoreAmount.gameObject.SetActive(showHighscore);
		highscoreAmount.text = PlayerPrefsUtil.Highscore.ToCustomString();
	}
}
