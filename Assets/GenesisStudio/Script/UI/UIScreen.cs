using System;
using System.Collections;
using UnityEngine;

public abstract class UIScreen : MonoBehaviour
{
    protected abstract IEnumerator OnShow();
    protected abstract IEnumerator OnHide();
    public abstract void Initialize();
    
    Coroutine _activeCoroutine;

    protected bool _hideOnStart = true;
    
    public bool HideOnStart => _hideOnStart;
    
    public virtual void Show()
    {
        _activeCoroutine = StartCoroutine(Wrapper(OnShow));
    }

    public virtual void Hide()
    {
        _activeCoroutine = StartCoroutine(Wrapper(OnHide));
    }

    private IEnumerator Wrapper(Func<IEnumerator> coroutine)
    {
        yield return StartCoroutine(coroutine());
        _activeCoroutine = null;
    }
}