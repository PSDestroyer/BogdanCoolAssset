using System;
using PlatformCharacterController;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] public float damage;

    public bool active;
    
    
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.ApplyDamage(damage);
            Debug.Log("HitBox hit " + other.name);
        }
    }
}
