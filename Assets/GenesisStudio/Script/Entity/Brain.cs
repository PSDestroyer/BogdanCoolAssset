using System;
using UnityEngine;

namespace GenesisStudio
{
    [DisallowMultipleComponent]
    public abstract class Brain : MonoBehaviour
    {
        [field: SerializeField] protected Inventory Inventory { get; private set; }

        [field: SerializeField]  private float _speed;
        public float Speed
        {
            get => _speed;
            set
            {
                if (value < 0)
                {
                    _speed = 0;
                    return;
                }

                if (value > 100)
                {
                    _speed = 100;
                    return;
                }

                _speed = value;
            }
        }
        public Health Health
        {
            get
            {
                TryGetComponent(out Health health);
                return health;
            }
        }
        
        
        private void Start()
        {
            Speed = _speed;
            Initialize();
        }

        public bool Contains<T>(out T result) where T : Component
        {
            result = GetComponent<T>();
            return result;
        }
        
        protected abstract void Initialize();
        
        public abstract void Move(Vector3 direction);
        public abstract void SetPath(Path path);
        
        public abstract void AddItem(ItemData item);    
        public abstract void AddItem(ItemData item, int amount);    
        public abstract void RemoveItem(ItemData item);
        public abstract void RemoveItem(ItemData item, int amount);
        public virtual void OnGiveItem(ItemData item) { }
        public abstract bool HasItem(ItemData item);
        public abstract void SelectItem(int index);
        public abstract void SelectItem(ItemData data);
        
        public void GiveItem(ItemData item, Brain to, int amount = 1)
        {
            this.RemoveItem(item, amount);
            to.AddItem(item, amount);
        }
        public virtual bool CanAdd(Item item)
        {
            return Inventory.CanAdd(item);
        }
        public virtual Inventory.ItemInfo GetSelectedItem()
        {
            return Inventory.GetSelectedItem;
        }
        public Inventory.ItemInfo GetItemByData(ItemData data)
        {
            return Inventory.GetItemByData(data);
        }
        public void ClearInventory()
        {
            Inventory.Clear();
        }
    }
}