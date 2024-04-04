using UnityEngine;

public class Boot : MonoBehaviour
{
	private void Awake()
	{
		Debug.Log("[Boot] Start Game, open menu");
		GameManager.Instance.OpenMainMenu();
	}
}
