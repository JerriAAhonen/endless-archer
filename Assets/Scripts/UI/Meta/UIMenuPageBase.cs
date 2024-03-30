using UnityEngine;

public abstract class UIMenuPageBase : MonoBehaviour
{
	protected UIMainMenuController controller;
	protected bool openedAdditively;

	public void Init(UIMainMenuController controller)
	{
		this.controller = controller;
		gameObject.SetActive(false);
	}

	public virtual void Enter() { }
	public virtual void Exit() { }
	public virtual void OnFocusRestored() { }

	public void SetOpendedAdditively(bool additively)
	{
		openedAdditively = additively;
	}
}
