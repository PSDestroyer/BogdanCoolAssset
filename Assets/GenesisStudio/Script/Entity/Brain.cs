using System;
using UnityEngine;

namespace GenesisStudio
{
    [DisallowMultipleComponent]
    public abstract class Brain : MonoBehaviour, ICharacter
    {
        protected Inventory Inventory { get; private set; }

        [field: SerializeField] private float _speed;

        bool _isActive = false;

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
            Inventory = new Inventory();
            StartCoroutine(Inventory.Initialize(this));
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
        protected abstract void OnAddItem(ItemData item);    
        protected abstract void OnAddItem(ItemData item, int amount);    
        protected abstract void OnRemoveItem(ItemData item);
        protected abstract void OnRemoveItem(ItemData item, int amount);
        public virtual void OnGiveItem(ItemData item) { }
        public bool HasItem(ItemData item)
        {
            return Inventory.HasItem(item);
        }
        public abstract void SelectItem(int index);
        public abstract void SelectItem(ItemData data);
        
        public virtual void GiveItem(ItemData item, ICharacter to, int amount = 1)
        {
            this.OnRemoveItem(item, amount);
            to.Inventory().AddItems(item, amount);
        }
        public void AddItem(ItemData data)
        {
            OnAddItem(data);
            Inventory.AddItem(data);

            Debug.Log($"Added item {data.ItemName}");
            
        }

        public void AddItem(ItemData data, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                OnAddItem(data, amount);
                Inventory.AddItem(data);
            }
        }

        public void RemoveItem(ItemData data)
        {
            OnRemoveItem(data);
            Inventory.RemoveItem(data);
        }

        public void RemoveItem(ItemData data, int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                OnRemoveItem(data, amount);
                Inventory.RemoveItem(data);
            }
        }
        public virtual bool CanAdd(Item item)
        {
            return Inventory.CanAdd(item);
        }
        public virtual Inventory.ItemInfo SelectedItem()
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

        public void Controls(bool value)
        {
            _isActive = value;
        }

        Inventory ICharacter.Inventory()
        {
            return Inventory;
        }

        void ICharacter.Initialize()
        {
            Initialize();
        }

        public virtual bool GiveItem(ICharacter to, ItemData data)
        {
            if (Inventory.HasItem(data))
            {
                RemoveItem(data);
                to.Inventory().AddItem(data);
                return true;
            }
            return false;
        }
    }
}