using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PistonManager : MonoSingleton<PistonManager>
{
    [HideInInspector] public bool isObjectMoving, isActive;

    [SerializeField] private GameObject successPanel;

    [SerializeField] private List<Transform> _placedOrder = new List<Transform>();
    [SerializeField] private List<Transform> _orderedParts = new List<Transform>();

    // Start is called before the first frame update
    private void Start()
    {
        DOTween.Init().SetCapacity(5000, 50);
        SetOrderList();
        isActive = true;
    }
    
    private void SetOrderList()
    {
        foreach (Transform child in transform)
        {
            if (child.GetSiblingIndex() > 0 && child.GetSiblingIndex() < transform.childCount - 1)
                _placedOrder.Add(child);
        }
    }

    public bool CheckNextPart(Transform part)
    {
        return _placedOrder[_orderedParts.Count] == part;
    }
    
    public bool CheckLastPart(Transform part)
    {
        if (_orderedParts.Count <= 0 || !_orderedParts.Contains(part)) return true;
        return _placedOrder[_orderedParts.Count - 1] == part;
    }
    
    private void CheckObjectAmount()
    {
        if (_placedOrder.Count <= _orderedParts.Count)
        {
            isActive = false;
            successPanel.SetActive(true);
        }
    }
    
    public void OrderNewPart(Transform part)
    {
        _orderedParts.Add(part);
        CheckObjectAmount();
    }
    
    public void RemoveObject(Transform part)
    {
        _orderedParts.Remove(part);
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
