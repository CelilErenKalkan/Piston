using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float Speed = 1;
    private float _xAxis;
    private float _yAxis;

    private void Update()
    {
        if (!Input.GetMouseButton(1)) return;
        
        transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * Speed, -Input.GetAxis("Mouse X") * Speed, 0));
        _xAxis = transform.rotation.eulerAngles.x;
        _yAxis = transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(_xAxis, _yAxis, 0);
    }
}
