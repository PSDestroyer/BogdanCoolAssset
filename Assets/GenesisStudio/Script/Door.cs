using System;
using System.Collections;
using System.Collections.Generic;
using GenesisStudio;
using UnityEngine;
using UnityEngine.Serialization;

public class Door : MonoBehaviour, IInteractable
{
    public float openAngle = 90f;
    public float openSpeed = 2f;
    
    public bool CanInteract { get; set; } = true;
    [field: SerializeField] public bool Hold { get; set; }
    [field: SerializeField] public float HoldTime { get; set; }

    private bool _isOpen = false;
    private Player _player;
    private bool _isUnlocked;

    private void Start()
    {
        _isOpen = true;
    }

    public void Interact(object sender)
    {
        _isOpen = !_isOpen;
    }

    private void Update()
    {
        Vector3 currentRot = transform.localEulerAngles;
        
        
        if (currentRot.y < openAngle && _isOpen)
        {
            transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, openAngle, currentRot.z),
                openSpeed * Time.deltaTime);
        }
        else if (currentRot.y > 0 && !_isOpen)
        {
            transform.localEulerAngles = Vector3.Lerp(currentRot, new Vector3(currentRot.x, 0, currentRot.z),
                openSpeed * Time.deltaTime);
        }
    }

    public bool TryUnlock(ItemData key)
    {
        if (key == null) return false; 
        if (key.ItemType != ItemType.Key) return false;
        if(_isUnlocked) return false;
        Unlock();
        return true;
    }

    private void Unlock()
    {
        _isUnlocked = true;
    }
}
