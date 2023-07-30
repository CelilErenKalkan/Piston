using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float ScrollSensitivity = -10f;
    private const float RotationSpeed = 1f;
    private const float MovementSpeed = 5f;

    private const float MinFov = 15f;
    private const float MaxFov = 90f;

    private PistonManager _pistonManager;
    private Camera _mainCamera;
    private float _xAxis;
    private float _yAxis;

    private void Awake()
    {
        _mainCamera = Camera.main;
    }

    private void Start()
    {
        _pistonManager = PistonManager.Instance;
    }

    private void Update()
    {
        if (!_pistonManager.isActive) return;
        HandleCameraScroll();
        HandleCameraRotation();
        HandleCameraMovement();
    }

    private void HandleCameraScroll()
    {
        // Camera Scroll
        float fov = _mainCamera.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * ScrollSensitivity;
        fov = Mathf.Clamp(fov, MinFov, MaxFov);
        _mainCamera.fieldOfView = fov;
    }

    private void HandleCameraRotation()
    {
        // Camera Rotation
        if (Input.GetMouseButton(1)) // right mouse button
        {
            Vector2 rotationInput = new Vector2(-Input.GetAxis("Mouse Y") * RotationSpeed, Input.GetAxis("Mouse X") * RotationSpeed);

            _xAxis += rotationInput.x;
            _yAxis += rotationInput.y;

            _xAxis = Mathf.Clamp(_xAxis, -89f, 89f); // Limit vertical rotation to avoid flipping the camera

            transform.rotation = Quaternion.Euler(_xAxis, _yAxis, 0);
        }
    }

    private void HandleCameraMovement()
    {
        // Camera Movement
        if (Input.GetMouseButton(0)) // left mouse button
        {
            if (_pistonManager.isObjectMoving) return;

            Vector3 movementInput = new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0) * MovementSpeed * Time.deltaTime;

            // Translates to the opposite direction of mouse position.
            transform.Translate(movementInput);
        }
    }
}