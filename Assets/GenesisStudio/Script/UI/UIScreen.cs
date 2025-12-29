using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    public abstract void OnShow();
    public abstract void OnHide();

    public virtual void Show()
    {
        gameObject.SetActive(true);
        OnShow();
    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
        OnHide();
    }

    
}