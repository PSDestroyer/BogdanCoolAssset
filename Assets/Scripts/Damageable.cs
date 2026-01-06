using UnityEngine;

    public class Damageable : MonoBehaviour, IDamageable
    {
        private float _health;
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
        public void ApplyDamage(float damage)
        {
            Health -= damage;
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
