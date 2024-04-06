using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
	[SerializeField] private TextMeshPro label;

	private Transform lookAtTarget;

	public float ShowTime { get; private set; }

	private void OnEnable()
	{
		ShowTime = Time.time;
	}

	private void OnDisable()
	{
		lookAtTarget = null;
		ShowTime = 0f;
	}

	private void LateUpdate()
	{
		if (!lookAtTarget)
			return;

		transform.rotation = Quaternion.LookRotation((lookAtTarget.position - transform.position) * -1);
	}

	public void SetText(string text, Transform lookAtTarget)
	{
		this.lookAtTarget = lookAtTarget;
		label.text = text;
	}
}
