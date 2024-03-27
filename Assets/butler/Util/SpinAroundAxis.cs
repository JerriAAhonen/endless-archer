using UnityEngine;

public class SpinAroundAxis : MonoBehaviour
{
	[SerializeField] private Vector3 rotationDelta;

	private void Update()
	{
		transform.Rotate(rotationDelta * Time.deltaTime);
	}
}
