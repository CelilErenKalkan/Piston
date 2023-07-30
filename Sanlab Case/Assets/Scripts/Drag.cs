using DG.Tweening;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private enum ItemType
    {
        Rod,
        Bolt,
    }

    private Vector3 mOffset;
    private float mZCoordinate;

    private PistonManager _pistonManager;
    [SerializeField] private Transform target;
    [SerializeField] private Material targetMaterial;
    
    [SerializeField] private ItemType itemType;
    
    [SerializeField] private float minDistance = 1.0f;

    private void Start()
    {
        _pistonManager = PistonManager.Instance;
        
        if (target.TryGetComponent(out MeshRenderer meshRenderer)) 
            targetMaterial = meshRenderer.material;
    }
    private void OnMouseDown()
    {
        if (!_pistonManager.isActive) return;
        
        _pistonManager.isObjectMoving = true;
        
        if (GetDistance() < minDistance)
            _pistonManager.RemoveObject();
        
        mZCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = game object world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        // Get the mouse position in screen coordinates (pixel coordinates)
        Vector3 mouseScreenPos = Input.mousePosition;

        // Set the z-coordinate of the mouse position to the distance from the camera to the object
        mouseScreenPos.z = mZCoordinate;

        // Convert the mouse position from screen coordinates to world coordinates
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

        return mouseWorldPos;
    }

    private float GetDistance()
    {
        return Vector3.Distance(transform.position, target.position);
    }

    private void MoveTowardsTarget()
    {
        transform.position = GetMouseWorldPos() + mOffset;

        
        if (GetDistance() < minDistance)
        {
            transform.DOMove(target.position, 0.4f);
        }
        else
        {
            CheckTargetTransparency(GetDistance() <= minDistance * 4);
        }
    }
    
    private void OnMouseDrag()
    {
        MoveTowardsTarget();
    }

    private void OnMouseUp()
    {
        _pistonManager.isObjectMoving = false;
        
        if (GetDistance() < minDistance)
        {
            PistonManager.Instance.AddNewObject();
        }
    }

    private void CheckTargetTransparency(bool isInRange)
    {
        float targetAlpha = isInRange ? 0.25f : 0.0f;
        targetMaterial.DOFade(targetAlpha, 0.2f);
    }
}