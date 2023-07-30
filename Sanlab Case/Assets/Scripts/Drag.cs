using System.Collections;
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

    [SerializeField] private Transform target;
    [SerializeField] private Material targetMaterial;
    
    [SerializeField] private ItemType itemType;
    
    [SerializeField] private float minDistance;

    private void Start()
    {
        if (target.TryGetComponent(out MeshRenderer meshRenderer))
        {
            targetMaterial = meshRenderer.material;
        }
    }

    private void OnEnable()
    {
        Actions.Success += OnLevelEnd;
    }

    private void OnDisable()
    {
        Actions.Success -= OnLevelEnd;
    }

    private void OnLevelEnd()
    {
        enabled = false;
        if (itemType != ItemType.Bolt)
            gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        mZCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = game object world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        // pixel coordinates (x,y)
        var mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoordinate;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        transform.position = GetMouseWorldPos() + mOffset;

        var distance = Vector3.Distance(transform.position, target.position);
        if (distance < minDistance)
        {
            switch (itemType)
            {
                case ItemType.Rod:
                    transform.DOMove(target.position, 0.3f);
                    return;
                case ItemType.Bolt:
                    transform.DOMove(target.position, 0.3f);
                    return;
                default:
                    return;
            }
        }
        
        CheckTargetTransparency(distance <= minDistance * 3);
    }

    private void CheckTargetTransparency(bool isInRange)
    {
        var color = targetMaterial.color;
        color.a = isInRange ? 0.25f : 0.0f;
        targetMaterial.DOColor(color, 0.2f);
    }
}