using System.Collections;
using PlatformCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Ability : MonoBehaviour
{
    [field: SerializeField] public float Cooldown { get; private set; }
    [field: SerializeField, Min(0)] public float GasUse { get; private set; }
    
    Coroutine C_active;
    protected MovementCharacterController _controller;
    protected Animator _animator;
    private GasMeter _gasContainer;
    
    
    protected abstract void Initialize();

    public void Initialize(MovementCharacterController controller)
    {
        _controller = controller;
        _animator = _controller.PlayerAnimator;
        _gasContainer = _controller.GasContainer;
        
        Initialize();
    }
    protected abstract IEnumerator C_Use();

    public virtual void Use(InputAction.CallbackContext context)
    {
        C_active ??= StartCoroutine(UseWrapper());
    }
    
    public void Use()
    {
        C_active ??= StartCoroutine(UseWrapper());
    }
    
    private IEnumerator UseWrapper()
    {
        yield return C_Use();
        yield return new WaitForSeconds(Cooldown);
        _gasContainer.Gas -= GasUse;
        C_active = null;
    }
}
