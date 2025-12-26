using System;
using System.Collections;
using GenesisStudio;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : CharacterMotor
{
    [field: SerializeField] private float GrabDistance;
    [field: SerializeField] private float ThrowForce;
    [field: SerializeField] public Hands hands { get; private set; }


    [SerializeField] private float InteractDistance;

    private Rigidbody _currentObject;
    private bool _isGrabling;
    private Coroutine _holdingCoroutine;
    private bool _stop;
    private ItemData _targetItem;
    private Inventory.ItemInfo SelectedItem => GetSelectedItem();
    public ItemData TargetItem => _targetItem;



    [Serializable] public class Hands
    {
        [field: SerializeField] public Transform RightHand { get; private set; }
        [field: SerializeField] public Transform LeftHand { get; private set; }
        [field: SerializeField] public Transform Both { get; private set; }
    }

    protected override void Initialize()
    {
        base.Initialize();

        _input.Subscribe(Needs.Interact, Interact);
        _input.Subscribe(Needs.Player_Select, ChangeItem);
        _input.Subscribe(Needs.Use, Input_UseItem);
        _interactDistance = InteractDistance;

    }

    public void SetTargetItem(ItemData target)
    {
        _targetItem = target;
    }

    protected virtual void Interact(InputAction.CallbackContext ctx)
    {
        var cam = _cameraMotor.camera;
        if (ctx.action.IsPressed())
        {
            if (Physics.Raycast(_cameraMotor.ray, out RaycastHit hit, _interactDistance))
            {
                if (hit.transform.gameObject.TryGetComponent(out CompleteActiveQuest completeQuest))
                {
                    completeQuest.Interact(this);
                    return;
                }

                if (hit.transform.gameObject.GetComponent<IInteractable>() != null)
                {
                    var interactables = hit.transform.gameObject.GetComponents<IInteractable>();
                    foreach (var interactable in interactables)
                    {
                        if (!interactable.CanInteract) return;
                        if (interactable.Hold)
                        {
                            _holdingCoroutine ??= StartCoroutine(BeginHolding(interactable.HoldTime, interactable));
                            return;
                        }

                        interactable.Interact(this);
                        CheckForInteract(interactable);
                    }

                    return;
                }

                if (hit.transform.gameObject.TryGetComponent(out IGrabable grabable) &&
                    hit.transform.TryGetComponent(out Rigidbody rb))
                {
                    if (_currentObject != null) return;
                    grabable.Grab(hands.Both);
                    _currentObject = rb;
                    _isGrabling = true;

                    _input.Subscribe(Needs.Cancel, Drop);
                    _input.Subscribe(Needs.Fire, Throw);
                    _input.Subscribe(Needs.Rotate, RotateCurrentObject);
                    _input.Unsubscribe(Needs.Player_Select);
                    return;
                }
            }
        }
        else
        {
            if (_holdingCoroutine != null)
                StopCoroutine(_holdingCoroutine);
            _holdingCoroutine = null;
            // GameManager.Instance.UI_holdingValue = 0;
        }
    }

    private void CheckForInteract(IInteractable interactable)
    {
        if (QuestManager.Instance.HasActiveQuest(out var ActiveQuest))
        {
            if (ActiveQuest is InteractWithQuest iwq)
            {
                iwq.CheckForComplete(interactable);
            }
        }
    }

    private IEnumerator BeginHolding(float time, IInteractable interactable)
    {
        // GameManager.Instance.UI_holdingValue = 0;

        float timer = 0;
        float value = 0;
        while (timer <= time)
        {
            timer += Time.deltaTime;
            value = Time.deltaTime / time;
            // GameManager.Instance.UI_holdingValue += value;
            yield return null;
        }

        interactable.Interact(this);
        CheckForInteract(interactable);
        if (SelectedItem != null)
        {
            if (_targetItem != null)
            {
                if (SelectedItem.Data == _targetItem)
                {
                    if (interactable is AIBrain brain &&
                        QuestManager.Instance.ActiveQuestGO.DataParams.Target_npc == brain)
                    {
                        GiveItem(_targetItem, brain);
                    }

                    _targetItem = null;
                }
            }
        }

        _holdingCoroutine = null;
        // GameManager.Instance.UI_holdingValue = 0;
    }

    private void FixedUpdate()
    {
        ExecuteGrabling();
    }


    private void ExecuteGrabling()
    {
        if (_currentObject)
        {
            Vector3 targetPosition = hands.Both.position;
            Vector3 moveDirection = (targetPosition - _currentObject.position) * 10f;
            _currentObject.linearVelocity = moveDirection;
        }
    }

    private void Drop(InputAction.CallbackContext ctx)
    {
        if (_currentObject)
        {
            _currentObject = null;

            _input.Unsubscribe(Needs.Cancel);
            _input.Unsubscribe(Needs.Fire);
            _input.Unsubscribe(Needs.Rotate);
            _input.Subscribe(Needs.Player_Select, ChangeItem);

        }
    }

    private void Throw(InputAction.CallbackContext ctx)
    {
        if (_currentObject)
        {
            _currentObject.AddForce(_cameraMotor.camera.transform.forward * ThrowForce, ForceMode.Impulse);
            _currentObject = null;

            _input.Unsubscribe(Needs.Cancel);
            _input.Unsubscribe(Needs.Fire);
            _input.Unsubscribe(Needs.Rotate);
            _input.Subscribe(Needs.Player_Select, ChangeItem);

        }
    }

    private void RotateCurrentObject(InputAction.CallbackContext ctx)
    {
        var direction = ctx.ReadValue<Vector2>(); // -1, 0, 1

        _currentObject.transform.Rotate(Vector3.up, 90 * direction.x);
    }

    public bool TryGiveTargetItem(Brain to, out ItemData givenItem)
    {
        givenItem = null;
        if (SelectedItem == null) return false;
        if (SelectedItem.Data == _targetItem)
        {
            givenItem = _targetItem;
            GiveItem(SelectedItem.Data, to);
            return true;
        }

        return false;
    }
}

