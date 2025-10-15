using UnityEngine;
using UnityEngine.InputSystem;

public class MovementManager : MonoBehaviour
{
    private InputSystem_Actions inputSystem;
    private Rigidbody rb;

    [SerializeField] private Camera cam;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float lookSensitivity = 2f;
    [SerializeField] private float pitchLimit = 80f;

    private float pitch = 0f; // Camera pitch (up/down rotation)

    private void Awake()
    {
        inputSystem = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        inputSystem.Enable();
    }

    private void OnDisable()
    {
        inputSystem.Disable();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        Move();
        Look();
    }

    private void Move()
    {
        Vector2 moveInput = inputSystem.Player.Move.ReadValue<Vector2>();
        Vector3 move = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized;
        rb.linearVelocity = new Vector3(move.x * moveSpeed, rb.linearVelocity.y, move.z * moveSpeed);
    }

    private void Look()
    {
        Vector2 lookDelta = inputSystem.Player.Look.ReadValue<Vector2>();

        // Rotate player around Y axis (horizontal look)
        transform.Rotate(Vector3.up * lookDelta.x * lookSensitivity * Time.deltaTime);

        // Adjust and clamp camera pitch (vertical look)
        pitch -= lookDelta.y * lookSensitivity * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

        if (cam != null)
        {
            cam.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
    }
}