using System;
using System.Collections;
using GenesisStudio;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    private CanvasGroup cg;
    protected CanvasGroup _canvasGroup
    {
        get
        {
            if (cg == null) 
                TryGetComponent(out cg);
            
            if(cg == null)
                cg = gameObject.AddComponent<CanvasGroup>();
            
            return cg;
        }
    }
    
    protected abstract IEnumerator OnShow();
    protected abstract IEnumerator OnHide();
    public abstract void Initialize();
    
    protected UIManager _uiManager => UIManager.Instance;
    Coroutine _activeCoroutine;

    [SerializeField] protected bool _hideOnStart = true;
    public bool isActive;

    public bool HideOnStart => _hideOnStart;
    
    public CanvasGroup CanvasGroup { get => _canvasGroup; }


    public virtual void Show()
    {
        _activeCoroutine = StartCoroutine(Wrapper(OnShow));
        isActive = true; 
    }

    public virtual void Hide()
    {
        _activeCoroutine = StartCoroutine(Wrapper(OnHide));
        isActive = false;
    }

    private IEnumerator Wrapper(Func<IEnumerator> coroutine)
    {
        yield return StartCoroutine(coroutine());
        _activeCoroutine = null;
    }
    
    
}