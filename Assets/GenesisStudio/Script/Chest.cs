using GenesisStudio;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
public class Chest : MonoBehaviour, IInteractable
{
    [SerializeField] private UnityEvent onChestOpen;
    public bool _isOpen = false;
    private Animator _animator;

    public bool CanInteract { get => !_isOpen; }
    [field: SerializeField] public bool Hold { get; set; }
    [field: SerializeField] public float HoldTime { get; set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    public void Interact(object sender)
    {
        if(_isOpen) return;
        if (Needs.IsSameTypeAs<Player>(sender))
        {
            onChestOpen?.Invoke();
            _isOpen = true;
            _animator.SetTrigger(_isOpen ? "Open" : "Close");
        }
    }
}
