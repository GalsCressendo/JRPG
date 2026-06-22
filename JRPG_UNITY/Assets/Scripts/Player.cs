using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputAction playerMoveAction;
    private InputAction playerLookAction;
    private InputAction playerInteractAction;
    private Vector2 playerMoveAmount;
    private Vector2 playerLookAmount;

    [SerializeField] Transform playerCamera;
    [SerializeField] private float playerWalkSpeed = 5f;
    [SerializeField] private float playerRotateDamp = 0.1f;
    [SerializeField] private float mouseLookSensitivity = 0.15f;
    [SerializeField] private float gamepadLookSpeed = 120f;
    [SerializeField] private float minimumCameraPitch = -35f;
    [SerializeField] private float maximumCameraPitch = 65f;

    [SerializeField] private InputActionAsset InputActions;
    [SerializeField] CharacterController playerCharacterController;

    private float turnSmoothingVelocity;
    private float cameraYaw;
    private float cameraPitch;

    private void OnEnable()
    {
        InputActions.FindActionMap("Player").Enable();
    }

    private void OnDisable()
    {
        InputActions.FindActionMap("Player").Disable();
    }

    private void Awake()
    {
        InputActionMap playerActionMap = InputActions.FindActionMap("Player", true);
        playerMoveAction = playerActionMap.FindAction("Move", true);
        playerLookAction = playerActionMap.FindAction("Look", true);
        playerInteractAction = playerActionMap.FindAction("Jump", true);

        cameraYaw = playerCamera.eulerAngles.y;
        cameraPitch = NormalizeAngle(playerCamera.eulerAngles.x);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        playerMoveAmount = playerMoveAction.ReadValue<Vector2>();
        playerLookAmount = playerLookAction.ReadValue<Vector2>();

        if (playerInteractAction.WasPressedThisFrame())
        {
            Interact();
        }

        ReadCameraLook();
        PlayerMoveAndRotate();
        ApplyCameraRotation();
    }

    private void ReadCameraLook()
    {
        bool isMouseInput = playerLookAction.activeControl?.device is Mouse;
        float lookMultiplier = isMouseInput ? mouseLookSensitivity : gamepadLookSpeed * Time.deltaTime;

        cameraYaw += playerLookAmount.x * lookMultiplier;
        cameraPitch -= playerLookAmount.y * lookMultiplier;
        cameraPitch = Mathf.Clamp(cameraPitch, minimumCameraPitch, maximumCameraPitch);
    }

    private void ApplyCameraRotation()
    {
        playerCamera.rotation = Quaternion.Euler(cameraPitch, cameraYaw, 0f);
    }

    private void PlayerMoveAndRotate()
    {
        Vector3 playerDir = new Vector3(playerMoveAmount.x, 0f, playerMoveAmount.y).normalized;

        if(playerDir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(playerDir.x, playerDir.z) * Mathf.Rad2Deg + cameraYaw;
            float smoothTargetAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVelocity, playerRotateDamp);

            transform.rotation = Quaternion.Euler(0f, smoothTargetAngle, 0f);

            Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            playerCharacterController.Move(moveDirection.normalized * playerWalkSpeed * Time.deltaTime);
        }
    }

    private static float NormalizeAngle(float angle)
    {
        return angle > 180f ? angle - 360f : angle;
    }

    private async void Interact()
    {
        await UniTask.WaitUntil(() => InteractibleManager.Instance != null);

        InteractibleManager.Instance.TriggerInteraction();
    }
}
