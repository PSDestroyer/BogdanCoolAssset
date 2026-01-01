using GenesisStudio;
using UnityEngine;

namespace Abilities
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] float _health;

        public float Health
        {
            get => _health;
            set
            {
                _health = value;
                if (_health <= 0)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            gameObject.SetActive(false);
        }
    }
}