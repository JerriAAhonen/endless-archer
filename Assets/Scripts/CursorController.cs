using UnityEngine;

public static class CursorController
{
	public static void OnStartLevel()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	public static void OnOpenMenu()
	{
		Cursor.lockState = CursorLockMode.Confined;
		Cursor.visible = true;
	}

	public static void OnEndLevel() => OnOpenMenu();
}
