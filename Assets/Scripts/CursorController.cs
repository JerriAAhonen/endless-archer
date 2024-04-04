using UnityEngine;

public static class CursorController
{
	public static void OnStartLevel() => Hide();
	public static void OnOpenMenu() => ConfineAndShow();
	public static void OnEndLevel() => ConfineAndShow();
	public static void OnPause(bool pause)
	{
		if (pause)
			ConfineAndShow();
		else
			Hide();
	}

	private static void ConfineAndShow()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	private static void Hide()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}
