using System.Collections;
using UnityEngine;

public abstract class Buff : MonoBehaviour
{
    Coroutine C_active;
    
    public abstract void Initialize();
    protected abstract IEnumerator C_Use();

    public void Use()
    {
        if (C_active != null)
            StopCoroutine(C_active);

        C_active = StartCoroutine(C_Use());
    }
}
