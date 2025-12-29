using System;
using UnityEngine;

namespace GenesisStudio
{
    [RequireComponent(typeof(Brain))]
    public class Health : MonoBehaviour
    {
        public float Heatlh
        {
            get => _heatlh;
            set
            {
                if (value < 0)
                {
                    _heatlh = 0;
                    Die();
                }
                _heatlh = value;
            }
        }
        
        
        
        private Brain _brain;
        private float _heatlh;

        private void Awake()
        {
            _brain = GetComponent<Brain>();
            Heatlh = 100f;
        }
        
        private void Die()
        {
            throw new NotImplementedException();
        }
       
    }
}