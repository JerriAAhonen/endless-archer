using UnityEngine;
using UnityEngine.UI;

public class UIPauseMenuView : UICoreViewBase
{
	[Space]
	[SerializeField] private GameObject hideContainer;
	[SerializeField] private Button buttonCorner;
	[SerializeField] private Button buttonResume;
	[SerializeField] private Button buttonMainMenu;
	[SerializeField] private UISettingsContent settingsContent;

	private void Awake()
	{
		buttonCorner.onClick.AddListener(OnPause);
		buttonResume.onClick.AddListener(OnPause);
		buttonMainMenu.onClick.AddListener(OnMainMenu);
		hideContainer.SetActive(false);
	}

	private void Update()
	{
		if (!GlobalGameState.GameOngoing)
			return;

		// TODO Switch to input manager events
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			OnPause();
		}
	}

	private void OnPause()
	{
		var paused = !GlobalGameState.Paused;
		GlobalGameState.SetGamePaused(paused);
		CursorController.OnPause(paused);
		AudioManager.Instance.SetMusicLowpass(paused);

		hideContainer.SetActive(paused);

		if (paused)
			settingsContent.Open();
		else
			settingsContent.Close();
	}

	private void OnMainMenu()
	{
		OnPause();
		GameManager.Instance.LevelController.GameOver();
	}
}
