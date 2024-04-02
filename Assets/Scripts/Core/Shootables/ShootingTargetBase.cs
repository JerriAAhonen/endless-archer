using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootingTargetBase : MonoBehaviour
{
	[SerializeField] private GameObject root;
	[SerializeField] new private Collider collider;
	[Space]
	[SerializeField] private LayerMask arrowMask;
	[SerializeField] protected float labelOffset;

	private void LateUpdate()
	{
		transform.rotation = Quaternion.identity;
	}

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
		root.SetActive(active);
		collider.enabled = active;
	}

	protected abstract void OnShot();
}
