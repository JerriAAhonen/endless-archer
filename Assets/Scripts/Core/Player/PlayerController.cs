using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private GameObject root;
	[SerializeField] new private Collider collider;
	[SerializeField] private Bow bow;
	[SerializeField] private Transform rotationContainer;
	[SerializeField] private Transform camTm;
	[Header("Layers")]
	[SerializeField] private LayerMask obstacleLayer;

	// Events
	private EventBinding<Event_PauseGame> pauseGameBinding;

	private LevelController controller;

	// Settings
	private float aimSensitivity;

	// Input
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
		pauseGameBinding = new EventBinding<Event_PauseGame>(OnPause);
		EventBus<Event_PauseGame>.Register(pauseGameBinding);

		PlayerPrefsUtil.PlayerPrefsUpdated += OnAimSensitivityUpdated;
	}

	private void OnDisable()
	{
		EventBus<Event_PauseGame>.Deregister(pauseGameBinding);

		PlayerPrefsUtil.PlayerPrefsUpdated -= OnAimSensitivityUpdated;
	}

	private void Update()
	{
		if (ControlsEnabled)
		{
			// TODO Switch to input manager events
			float mouseX = Input.GetAxis("Mouse X") * aimSensitivity * Time.deltaTime * 100;
			float mouseY = Input.GetAxis("Mouse Y") * aimSensitivity * Time.deltaTime * 100;

			xRotation -= mouseY;
			xRotation = Mathf.Clamp(xRotation, -90f, 90f);

			yRotation += mouseX;
			yRotation = Mathf.Clamp(yRotation, -90f, 90f);
		}
		else if (!GlobalGameState.Paused) // Don't reset rotation when pausing the game
		{
			// Reset player rotation to 0 for main menu
			if (xRotation.Approximately(0f) && yRotation.Approximately(0f))
				return;

			xRotation = Mathf.MoveTowards(xRotation, 0f, Time.deltaTime * 10);
			yRotation = Mathf.MoveTowards(yRotation, 0f, Time.deltaTime * 10);
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

	public void OnGameOver()
	{
		SetActive(false);
		ControlsEnabled = false;
		bow.OnLevelEnded();
	}

	private void SetActive(bool active)
	{
		collider.enabled = active;
	}

	private void OnAimSensitivityUpdated()
	{
		aimSensitivity = PlayerPrefsUtil.AimSensitivity;
	}

	private void OnPause(Event_PauseGame @event)
	{
		ControlsEnabled = !@event.pause;
	}
}
