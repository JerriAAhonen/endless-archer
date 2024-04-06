using UnityEngine;

public class UICoreController : UICoreViewBase
{
	[Space]
	[SerializeField] private UIReticle reticle;
	[SerializeField] private UIScoreView score;
	[SerializeField] private UIGameOverView gameOver;
	[SerializeField] private UIPauseMenuView pauseMenu;

	public UIScoreView Score => score;
	public UIGameOverView GameOver => gameOver;

	public void OnStartLevel()
	{
		reticle.SetVisible(true);
		score.SetVisible(true);
		pauseMenu.SetVisible(true);
	}

	public void OnGameOver()
	{
		reticle.SetVisible(false);
		score.SetVisible(false);
		pauseMenu.SetVisible(false);
	}
}
