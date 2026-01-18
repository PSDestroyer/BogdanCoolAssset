using System;
using Unity.Cinemachine;
using UnityEngine;

    public class Damageable : MonoBehaviour, IDamageable
    {

        [SerializeField] protected bool CanDie;
        [SerializeField, Range(1,100f)] protected float _health;
        public float Health
        {
            get => _health;
            set
            {
                if(!CanDie)
                {
                    _health = -1;
                    return;
                }
                
                if (value < 0)
                {
                    _health = 0;
                    if(CanDie) Die();
                }
                _health = value;
            }
        }

        protected virtual void Start()
        {
            if(CanDie)
                Health = _health;
            else 
                _health = -1;
        }

        public virtual void ApplyDamage(float damage)
        {
            Health -= damage;
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
