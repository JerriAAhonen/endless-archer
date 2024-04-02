using UnityEngine;

public class Arrow : MonoBehaviour
{
	[SerializeField] private float forceMultiplier = 5f;
	[SerializeField] private Transform model;

	private Rigidbody rb;
	private bool shot;
	private bool hit;

	private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.isKinematic = true;
	}

	private void LateUpdate()
	{
		if (!shot) return;
		if (hit) return;
		if (rb.velocity.Approximately(Vector3.zero)) return;

		transform.rotation = Quaternion.LookRotation(rb.velocity);
	}
	
	private void OnCollisionEnter(Collision collision)
	{
		hit = true;
		rb.isKinematic = true;
		transform.parent = collision.transform;
	}

	public void Shoot(float force)
	{
		transform.parent = null;

		rb.isKinematic = false;
		rb.AddForce(force * forceMultiplier * transform.forward, ForceMode.Impulse);
		shot = true;
		hit = false;
	}
}
