using HalvaStudio.Save;
using UnityEngine;

namespace GenesisStudio
{
    [RequireComponent(typeof(ICharacter))]
    public class Health : MonoBehaviour
    {
        ICharacter _character;
        [SerializeField, Range(0,100f)] private float _heatlh;
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
                GameEventBus.Instance.OnHealthChanged?.Invoke(_heatlh);
            }
        }
        
        

        private void Start()
        {
            _character = GetComponent<ICharacter>();
            Heatlh = SaveManager.Instance.saveData.health;
            
        }
        
        private void Die()
        {
            Debug.Log($"{gameObject.name} is dead");
        }
       
    }
}