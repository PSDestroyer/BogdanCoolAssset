using System;
using System.Collections;
using GenesisStudio;
using HalvaStudio.Save;
using PlatformCharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Ability : MonoBehaviour
{
    [field: SerializeField] public AbilityData Data { get; private set; }
    
    
    //player stats
    protected MovementCharacterController _controller;
    protected Animator _animator;
    private GasMeter _gasContainer;
    
    private int _level;
    private float _gasUse;
    private float _cooldown;
    Coroutine C_active;


    public bool fromSave = false;

    public float Damage
    {
        get => Data.Damage * Mathf.Pow(1.15f, _level);
    }
    public float Cooldown
    {
        get => Data.Cooldown / (1 + _level * 0.15f);
    }
    
    public int Level => _level;

    public float GasUse
    {
        get => Data.GasUse * Mathf.Pow(0.85f, _level);
    }

    public int Price
    {
        get => Data.Price * (int)Mathf.Pow(1.15f, _level);
    }
    
    public void CheckForDestroy()
    {
        if (SaveManager.Instance.saveData.Contains(this))
        {
            Destroy(gameObject);
        }
    }

    protected abstract void Initialize();

    public void Initialize(MovementCharacterController controller)
    {
        _controller = controller;
        _animator = _controller.PlayerAnimator;
        _gasContainer = _controller.GasContainer;

        _gasUse = GasUse;
        _level = Data.Level;
        _cooldown = Cooldown;
        
        InputManager.Instance.Subscribe(Data.ActionName, Use);
        
        Initialize();
    }
    protected abstract IEnumerator Action();

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
        if(_gasContainer.Gas < _gasUse)
            yield break;
        
        _gasContainer.Gas -= _gasUse;
        _controller.GasUpdate();
        yield return Action();
        GameEventBus.Instance.OnUseAbility?.Invoke(Data);
        yield return new WaitForSeconds(_cooldown);
        C_active = null;
    }

    public void Upgrade()
    {
        _controller.Collected -= Price;
        
        if(_level >= Data.MaxLevel)
            return;
        
        _level++;
    }

    private void OnTriggerEnter(Collider other)
    {
        var Player = GameManager.Instance.Player as MovementCharacterController;
        Player.AbilityManager.AddAbility(this);
        gameObject.SetActive(false);
    }
    
}
