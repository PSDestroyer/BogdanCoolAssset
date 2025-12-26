using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace GenesisStudio
{
    [Serializable]
    public class Inventory  
    {
        [field: SerializeField]public int MaxItems { get; private set; } = 2;
        public int CurrentItems => items.Count;
        private ItemInfo _selectedItem;
        public List<ItemInfo> items = new List<ItemInfo>();
        public Action<ItemData> OnUpdateInventory { get; private set; }
        public ItemInfo GetSelectedItem => _selectedItem;
        public ItemInfo GetItemByData(ItemData data) => items.FirstOrDefault(i => i.Data == data);


        public IEnumerator Initialize(List<ItemInfo> default_items = null)
        {
            if (default_items != null)
            {
                items = default_items;
            }
            yield return null;
        }

        public bool CanAdd(Item item)
        {
            return CurrentItems < MaxItems || items.Any(i => i.Data == item.data && (i.Data.IsStackable ? i.Count + 1 < i.Data.MaxStackSize : i.Count < 1 && i.Count >= 0));
        }

        public void AddItem(ItemData data)
        {
            if(data == null) return;
            if (items.Any(i => i.Data == data) && data.IsStackable)
            {
                var item = items.First(i => i.Data == data);
                if (item.Count < data.MaxStackSize)
                    item.Count++;
            }
            else
            {
                items.Add(new ItemInfo(data));
                Select(data);
            }

            //Debug.Log($"Add item {data.ItemName} to inventory");
        }

        public void AddItems(ItemData data, int amount)
        {
            if (data == null) return;
            if(amount < 0) amount = 0;
            for (int i = 0; i < amount; i++)
            {
                AddItem(data);
            }
        }

        public bool HasItem(ItemData data) => items.FirstOrDefault(i => i.Data == data) != null;
        public void RemoveItem(ItemData data)
        {
            if (data == null) return;
            if (items.Any(i => i.Data == data))
            {
                var item = items.First(i => i.Data == data);
                if (item.Count > 0)
                {
                    item.Count--;
                    if (item.Count <= 0)
                    {
                        items.Remove(item);
                        SelectAt(0);
                    }
                }
            }

            //Debug.Log($"Remove item {data.ItemName} from inventory");
        }   

        public void Select(ItemData toSelect)
        {
            _selectedItem = GetItemInfo(toSelect);
            //Debug.Log($"Selected {toSelect.ItemName}");
        }

        public void SelectAt(int index)
        {
            if(index < 0 || index >= items.Count) return;
            var item = items[index];
            if (item.IsEmpty())
            {
                _selectedItem = null;
                return;
            }
            Select(items[index].Data);
        }

        public ItemInfo GetItemInfo(ItemData data)
        {
            return items.FirstOrDefault(i => i.Data == data);
        }

        public void RemoveItems(ItemData item, int amount)
        {
            ItemInfo target = GetItemByData(item);
            int targetAmount = target.Count;

            if (targetAmount - amount < 0) return;

            for (int i = 0; i < amount; i++)
            {
                RemoveItem(item);
            }
        }

        public void Clear()
        {
            items.Clear();
        }

        [Serializable]
        public class ItemInfo
        {
            [field: SerializeField] public ItemData Data { get; private set; }
            [field: SerializeField] public int Count { get; set; }
            [HideInInspector] public Inventory inventory;

            public ItemInfo(ItemData data)
            {
                Data = data;
                Count = 1;
            }
            public bool IsEmpty()
            {
                return Count == 0 || Data == null;
            }
        } 
    }
}
