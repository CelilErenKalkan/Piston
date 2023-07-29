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
    private Vector3 _originPos, _originRot;
    [SerializeField] private ItemType itemType;
    private bool _shouldStopRotate, _hasStuck;
    [SerializeField] private float minDistance;

    private void Start()
    {
        var transform1 = transform;
        _originPos = transform1.position;
        _originRot = transform1.eulerAngles;
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

        //if (itemType == ItemType.Pimple)
        //mousePoint.y = transform.position.y;

        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseDrag()
    {
        if (_hasStuck) return;
        transform.position = GetMouseWorldPos() + mOffset;
        if (!_shouldStopRotate) transform.LookAt(target);

        var distance = Vector3.Distance(transform.position, target.position);
        if (distance < minDistance)
        {
            switch (itemType)
            {
                case ItemType.Rod:
                    if (_hasStuck) return;
                    _hasStuck = true;
                    transform.DOMove(target.position, 0.5f).OnComplete(GetTheTarget);
                    return;
                case ItemType.Bolt:
                    transform.DOMove(target.position, 0.2f);
                    transform.GetChild(0).DORotate(target.eulerAngles, 0.2f);
                    transform.GetChild(0).DOScale(target.lossyScale, 0.2f).OnComplete(StayOnTarget);
                    return;
                default:
                    return;
            }
        }
    }

    private void GetTheTarget()
    {
        target.SetParent(transform.GetChild(0));
        Return();
        _shouldStopRotate = true;
    }

    private void StayOnTarget()
    {
        transform.SetParent(target.parent);
    }

    private void OnMouseUp()
    {
        if (!_hasStuck)
        {
            Return();
        }
    }

    private void Return()
    {
        transform.DOMove(_originPos, 0.2f);
        transform.DORotate(_originRot, 0.2f);
    }
}