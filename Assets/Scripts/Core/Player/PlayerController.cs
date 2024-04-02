using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private GameObject root;
	[SerializeField] new private Collider collider;
	[Space]
	[SerializeField] private LayerMask obstacleLayer;
	[Space]
	[SerializeField] private Bow bow;
	[SerializeField] private Transform rotationContainer;
	[SerializeField] private Transform camTm;

	private LevelController controller;

	private float aimSensitivity;

	private float xRotation;
	private float yRotation;

	public bool ControlsEnabled { get; private set; }
	public Transform CameraTransform => camTm;

	private void Awake()
	{
		SetActive(false);
	}

	public void Init(LevelController controller)
	{
		this.controller = controller;
		bow.Init(this);
		OnAimSensitivityUpdated();
	}

	private void OnEnable()
	{
		PlayerPrefsUtil.PlayerPrefsUpdated += OnAimSensitivityUpdated;
	}

	private void OnDisable()
	{
		PlayerPrefsUtil.PlayerPrefsUpdated -= OnAimSensitivityUpdated;
	}

	private void Update()
	{
		if (ControlsEnabled)
		{
			float mouseX = Input.GetAxis("Mouse X") * aimSensitivity * Time.deltaTime * 100;
			float mouseY = Input.GetAxis("Mouse Y") * aimSensitivity * Time.deltaTime * 100;

			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			yRotation += mouseX;
			yRotation = Mathf.Clamp(yRotation, -90f, 90f);
		}
		else
		{
			if (xRotation.Approximately(0f) && yRotation.Approximately(0f))
				return;

			xRotation = Mathf.MoveTowards(xRotation, 0f, Time.deltaTime);
			yRotation = Mathf.MoveTowards(yRotation, 0f, Time.deltaTime);
		}

		rotationContainer.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
		transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
	}

	private void OnCollisionEnter(Collision collision)
	{
		Debug.Log("Player collision");
		if (BitMaskUtil.MaskContainsLayer(obstacleLayer, collision.gameObject.layer))
		{
			controller.GameOver();
		}
	}

	public void OnStartLevel()
	{
		SetActive(true);
		ControlsEnabled = true;
		bow.OnStartLevel();
	}

	public void OnLevelEnded()
	{
		SetActive(false);
		ControlsEnabled = false;
		bow.OnLevelEnded();
	}

	private void SetActive(bool active)
	{
		root.SetActive(active);
		collider.enabled = active;
	}

	private void OnAimSensitivityUpdated()
	{
		aimSensitivity = PlayerPrefsUtil.AimSensitivity;
	}
}
