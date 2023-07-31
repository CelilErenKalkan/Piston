using DG.Tweening;
using UnityEngine;

public class DragAndDropController : MonoBehaviour
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
    private const float GettingInPlaceDuration = 0.8f;

    private void Start()
    {
        _pistonManager = PistonManager.Instance;

        if (target.TryGetComponent(out MeshRenderer meshRenderer))
            targetMaterial = meshRenderer.material;
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
    
    private void SetTargetTransparency(float targetAlpha, float duration)
    {
        targetMaterial.DOFade(targetAlpha, duration);
    }

    private void CheckTargetTransparency(bool isInRange)
    {
        float targetAlpha = isInRange ? 0.5f : 0.0f;
        SetTargetTransparency(targetAlpha, 0.2f);
    }

    private void PlaceWithRotation()
    {
        transform.DOMove(target.GetChild(0).position, GettingInPlaceDuration);
        if (itemType == ItemType.Bolt)
            transform.DOLocalRotate(new Vector3(0, 180, 0), GettingInPlaceDuration, RotateMode.FastBeyond360).SetRelative(true)
                .SetEase(Ease.Linear);
    }

    private void MoveTowardsTarget()
    {
        transform.position = GetMouseWorldPos() + mOffset;
        
        if (GetDistance() < minDistance && _pistonManager.CheckNextPart(transform))
        {
            transform.DOMove(target.position, GettingInPlaceDuration)
                .OnComplete(
                    () =>
                    {
                        SetTargetTransparency(0.0f, 0.2f);
                        
                        if (target.childCount > 0)
                        {
                            PlaceWithRotation();
                        }
                    });
        }
        else
        {
            CheckTargetTransparency(GetDistance() <= minDistance * 4);
        }
    }
    
    private void OnMouseDown()
    {
        if (!_pistonManager.isActive || !_pistonManager.CheckLastPart(transform)) return;
        
        _pistonManager.isObjectMoving = true;

        if (GetDistance() < minDistance)
            _pistonManager.RemoveObject(transform);

        mZCoordinate = Camera.main.WorldToScreenPoint(transform.position).z;

        // Store offset = game object world pos - mouse world pos
        mOffset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseDrag()
    {
        if (!_pistonManager.isActive || !_pistonManager.CheckLastPart(transform)) return;
        MoveTowardsTarget();
    }

    private void OnMouseUp()
    {
        if (!_pistonManager.isActive || !_pistonManager.CheckLastPart(transform)) return;
        
        _pistonManager.isObjectMoving = false;

        if (GetDistance() < minDistance && _pistonManager.CheckNextPart(transform))
        {
            PistonManager.Instance.OrderNewPart(transform);
        }
    }
}