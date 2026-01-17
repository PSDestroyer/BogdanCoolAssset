using System;
using Unity.Cinemachine;
using UnityEngine;

    public class Damageable : MonoBehaviour, IDamageable
    {

        [SerializeField] private bool CanDie;
        [SerializeField, Range(1,100f)]private float _health;
        public float Health
        {
            get => _health;
            set
            {
                if(CanDie)
                {
                    _health = -1;
                    return;
                }
                
                if (value < 0)
                {
                    _health = 0;
                    Die();
                }
                _health = value;
            }
        }

        private void Start()
        {
            if(CanDie)
                Health = _health;
            else 
                _health = -1;
        }

        public void ApplyDamage(float damage)
        {
            Health -= damage;
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
