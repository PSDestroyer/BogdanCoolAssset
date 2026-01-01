using System;
using UnityEngine;

namespace GenesisStudio
{
    [RequireComponent(typeof(ICharacter))]
    public class Health : MonoBehaviour
    {
        ICharacter _character;
        private float _heatlh;
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
                GameEventBus.Instance.OnHealthChanged(_heatlh);
            }
        }
        
        

        private void Awake()
        {
            _character = GetComponent<ICharacter>(); 
            Heatlh = 100f;
        }
        
        private void Die()
        {
            throw new NotImplementedException();
        }
       
    }
}