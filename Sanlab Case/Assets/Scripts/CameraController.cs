using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float Speed = 1;
    private float _xAxis;
    private float _yAxis;
    
    float minFov = 15f;
    float maxFov = 90f;
    float sensitivity = -10f;

    private Camera _mainCamera;

    private void Awake() => _mainCamera = Camera.main;
    
    private void Update()
    {
        // Camera Scroll
        float fov = _mainCamera.fieldOfView;
        fov += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        fov = Mathf.Clamp(fov, minFov, maxFov);
        _mainCamera.fieldOfView = fov;
        
        // Camera Rotation
        if (Input.GetMouseButton(1)) // right mouse button
        {
            Vector3 newRotation = default;
            newRotation.x = Input.GetAxis("Mouse Y") * Speed;
            newRotation.y = -Input.GetAxis("Mouse X") * Speed;
            
            transform.Rotate(-newRotation);
            _xAxis = transform.rotation.eulerAngles.x;
            _yAxis = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(_xAxis, _yAxis, 0);
        }
        
        // Camera Movement
        if (Input.GetMouseButton(0)) // left mouse button
        {
            if (Manager.Instance.isObjectMoving) return;
            
            Vector3 newPosition = default;
            newPosition.x = Input.GetAxis("Mouse X") * Speed * 5 * Time.deltaTime;
            newPosition.y = Input.GetAxis("Mouse Y") * Speed * 5 * Time.deltaTime;
            
            // translates to the opposite direction of mouse position.
            transform.Translate(-newPosition);
        }
    }
}
