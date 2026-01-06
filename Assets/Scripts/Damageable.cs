using System;
using UnityEngine;

    public class Damageable : MonoBehaviour, IDamageable
    {
        [SerializeField, Range(1,100f)]private float _health;
        public float Health
        {
            get => _health;
            set
            {
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
            Health = _health;
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
