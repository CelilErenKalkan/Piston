using Blended;
using DG.Tweening;

public class Manager : MonoSingleton<Manager>
{
    public bool isObjectMoving;
    
    // Start is called before the first frame update
    private void Start()
    {
        DOTween.Init().SetCapacity(5000, 50);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
