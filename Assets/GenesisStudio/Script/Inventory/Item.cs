using UnityEngine;

namespace GenesisStudio
{
    public class Item : MonoBehaviour, IInteractable
    {
        public ItemData data;
        private Renderer _renderer;
        public Renderer Renderer => _renderer;

        public bool CanInteract
        {
            get
            {
                return true;
            }
        }
        [field: SerializeField] private int Amount;

        [field: SerializeField] public bool Hold { get; set; }
        [field: SerializeField] public float HoldTime { get ; set; }

        public void Interact(object sender)
        {
            if (sender is Brain b)
            {
                AddItemToBrain(b);
            }       
        }

        protected virtual void AddItemToBrain(Brain b)
        {
            if (b.CanAdd(this))
            {
                b.AddItem(data);
                Destroy(gameObject);
            }
        }

        private void Awake()
        {
            _renderer = gameObject.GetComponent<Renderer>();
        }

        private void Start()
        {
            ToggleOutline(false);
            Initialize();
        }

        protected virtual void Initialize()
        {
            
        }

        public void ToggleOutline(bool value)
        {
            if(value) 
                GameManager.Instance.OutlineManager.ShowOutline(_renderer, Color.green, 10);
            else 
                GameManager.Instance.OutlineManager.ClearOutline();
        }

    }
}
