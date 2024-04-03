using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;

public abstract class ShootingTargetBase : MonoBehaviour
{
	[SerializeField] private GameObject root;
	[SerializeField] new private Collider collider;
	[Space]
	[SerializeField] private LayerMask arrowMask;
	[SerializeField] protected float labelOffset;
	[Header("Randomization")]
	[SerializeField] private bool randomize;
	[Range(0f, 1f), Tooltip("1 = Always shown")]
	[ShowIf("randomize")][SerializeField] private float chanceToShow = 1f;
	[ShowIf("randomize")][SerializeField] private List<ShootingTargetBase> disableIfThisEnabled;

	private void OnCollisionEnter(Collision collision)
	{
		if (BitMaskUtil.MaskContainsLayer(arrowMask, collision.gameObject.layer))
		{
			OnShot();
			SetActive(false);
			Destroy(collision.gameObject);
		}
	}

	public void SetActive(bool active)
	{
		if (active && randomize && chanceToShow < 1f) // Should randomize whether to show this target
		{
			var rand = Random.Range(0f, 1f);
			if (rand > chanceToShow) // Should not show
			{
				Activate(false);
				return;
			}
			else // Should show, hide conflicting targets
			{
				foreach (var target in disableIfThisEnabled)
					target.SetActive(false);
			}
		}

		Activate(active);

		void Activate(bool activate)
		{
			root.SetActive(activate);
			collider.enabled = activate;
		}
	}

	protected abstract void OnShot();
}
