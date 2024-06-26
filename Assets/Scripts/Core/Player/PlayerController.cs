using UnityEngine;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private GameObject root;
	[SerializeField] new private Collider collider;
	[SerializeField] private Bow bow;
	[SerializeField] private Transform rotationContainer;
	[SerializeField] private Transform camTm;
	[SerializeField] private ParticleSystem playerCollisionPS;
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

	public bool ControlsEnabled => !GlobalGameState.Paused && GlobalGameState.GameOngoing;
	public bool BlockInput { get; private set; }

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
		OnAimSensitivityUpdated();
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

	private void OnTriggerEnter(Collider other)
	{
		//Debug.Log("Player TRIGGER enter");
		if (BitMaskUtil.MaskContainsLayer(obstacleLayer, other.gameObject.layer))
		{
			playerCollisionPS.Play();
			controller.GameOver();
		}
	}

	public void OnStartLevel()
	{
		SetActive(true);
		bow.OnStartLevel();
	}

	public void OnGameOver()
	{
		SetActive(false);
		bow.OnLevelEnded();
		BlockInput = false;
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
		// Block the user input when pausing the game
		if (@event.pause)
		{
			BlockInput = true;
		}
		// Wait for a tiny amount before re-enabling input
		else
		{
			LeanTween.delayedCall(0.01f, () => BlockInput = false);
		}
	}
}
