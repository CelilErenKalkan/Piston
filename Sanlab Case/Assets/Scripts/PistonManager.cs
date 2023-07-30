using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PistonManager : MonoSingleton<PistonManager>
{
    [HideInInspector] public int currentPartAmount, totalPartAmount;
    [HideInInspector] public bool isObjectMoving, isActive;

    [SerializeField] private GameObject successPanel;
    
    // Start is called before the first frame update
    private void Start()
    {
        DOTween.Init().SetCapacity(5000, 50);
        totalPartAmount = transform.GetChild(transform.childCount - 1).childCount - 1;
        isActive = true;
    }
    
    private void CheckObjectAmount()
    {
        if (currentPartAmount >= totalPartAmount)
        {
            isActive = false;
            successPanel.SetActive(true);
        }
    }
    
    public void AddNewObject()
    {
        currentPartAmount++;
        CheckObjectAmount();
    }
    
    public void RemoveObject()
    {
        currentPartAmount--;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
