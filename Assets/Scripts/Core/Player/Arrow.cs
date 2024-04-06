using System;
using UnityEngine;

public class Arrow : MonoBehaviour
{
	[SerializeField] private float forceMultiplier = 5f;
	[SerializeField] private Transform model;
	[SerializeField] private ParticleSystem trailPS;

	private EventBinding<Event_LevelEnded> levelEndedBinding;

	private Action<Arrow> releaseCallback;
	private Rigidbody rb;
	private bool shot;
	private bool hit;

	public void Init(Action<Arrow> releaseCallback)
	{
		this.releaseCallback = releaseCallback;

		if (!rb) 
			rb = GetComponent<Rigidbody>();
		rb.isKinematic = true;
		trailPS.gameObject.SetActive(false);
	}

	private void Awake()
	{
		levelEndedBinding = new EventBinding<Event_LevelEnded>(OnLevelEnded);
		EventBus<Event_LevelEnded>.Register(levelEndedBinding);
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

		trailPS.Stop(true, ParticleSystemStopBehavior.StopEmitting);
	}

	private void OnDestroy()
	{
		EventBus<Event_LevelEnded>.Deregister(levelEndedBinding);
	}

	public void Shoot(float force)
	{
		transform.parent = null;

		rb.isKinematic = false;
		rb.AddForce(force * forceMultiplier * transform.forward, ForceMode.Impulse);
		shot = true;
		hit = false;

		trailPS.gameObject.SetActive(true);
	}

	public void ReleaseToPool()
	{
		releaseCallback?.Invoke(this);
	}

	private void OnLevelEnded(Event_LevelEnded _)
	{
		releaseCallback?.Invoke(this);
	}
}
