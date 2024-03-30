using UnityEngine;

public class GameManager : PersistentSingleton<GameManager>
{
	[SerializeField] private UIMainMenuController mainMenuController;
	[SerializeField] private LevelController levelController;

	public LevelController LevelController => levelController;

	public void OpenMenu()
	{
		mainMenuController.SetVisible(true);
		mainMenuController.SwitchPage(MenuPageType.Main);

		CursorController.OnOpenMenu();
	}

	public void StartLevel()
	{
		mainMenuController.SetVisible(false);
		levelController.StartLevel();

		CursorController.OnStartLevel();
	}

	public void OnLevelEnded()
	{
		OpenMenu();
	}

	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#endif
		Application.Quit();
	}
}
