using System;
using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
	[SerializeField] private UILoadingCanvas loadingCanvas;
	[SerializeField] private UIMainMenuController mainMenuController;
	[SerializeField] private LevelController levelController;
	[SerializeField] private Camera mainCamera;

	public Camera MainCamera => mainCamera;
	public LevelController LevelController => levelController;

	public void Init()
	{
		loadingCanvas.SetVisible(true, () =>
		{
			OpenMainMenu();
			loadingCanvas.SetVisible(false, null);
		});
	}

	public void OpenMainMenu()
	{
		mainMenuController.SetActive(true);
		mainMenuController.SwitchPage(MenuPageType.Main);

		CursorController.OnOpenMenu();
	}

	public void StartLevel()
	{
		CursorController.OnStartLevel();

		loadingCanvas.SetVisible(true, () =>
		{
			mainMenuController.SetActive(false);
			levelController.StartLevel();
			loadingCanvas.SetVisible(false, null);
		});
	}

	public void ShowLoadingCanvas(bool show, Action onComplete) => loadingCanvas.SetVisible(show, onComplete);

	public void OnContinueAfterGameOver()
	{
		OpenMainMenu();
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}
}
