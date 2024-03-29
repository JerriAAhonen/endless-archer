using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	[SerializeField] private float forceMultiplier = 5f;

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

		var desiredRotation = Quaternion.LookRotation(rb.velocity);
		transform.rotation = desiredRotation;
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
		rb.AddForce(transform.forward * force * forceMultiplier, ForceMode.Impulse);
		shot = true;
		hit = false;
	}
}
