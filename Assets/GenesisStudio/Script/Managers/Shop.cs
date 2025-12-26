using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

namespace GenesisStudio
{
    /// <summary>
    /// This is abstract shop, you can use it in menu, gameplay, where you want
    /// in menu:
    ///     IInteractable shop;
    ///     Button ShopButton;
    ///
    ///     ShopButton.onClick.AddListner(() => Interact(this));
    /// etc.
    ///
    /// in gameplay:
    ///     in SomeShop.cs ...
    /// </summary>
    public class Shop : MonoBehaviour, IInteractable
    {
        [field: SerializeField] protected Canvas canvas;
        [field: SerializeField] protected Transform parent;
        [field: SerializeField] protected ShopObject prefab;
        [field: SerializeField] public List<ItemData> _items { get; private set; } = new List<ItemData>();
        private List<ShopObject> _objects = new List<ShopObject>();
        public bool CanInteract { get; set; } = true;
        [field: SerializeField] public bool Hold { get; set; }
        [field: SerializeField] public float HoldTime { get; set; }

        protected object interactUser;
        protected bool isActive;

        protected virtual void OnShopEnter()
        {
            InputManager.Instance.ChangeMap(Needs.UIMap);
            InputManager.Instance.playerInput.actions[Needs.Cancel].performed += Interact;
            _objects[0].Button.Select();
        }

        protected virtual void OnShopExit()
        {
            InputManager.Instance.playerInput.actions[Needs.Cancel].performed -= Interact;
            InputManager.Instance.ChangeMap(Needs.PlayerMap);
            interactUser = null;
            GameManager.Instance.Player.SelectItem(0);
        }
        
        public virtual void Interact(object sender)
        {
            interactUser = sender;
            if (Needs.IsSameTypeAs<AIBrain>(sender))
            {
                interactUser = sender as AIBrain;
                Debug.Log($"<color=green>Shop</color>: Interact User: {interactUser}");
                return;
            }
            if (Needs.IsSameTypeAs<Player>(sender))
            {
                interactUser = sender as Player;
                isActive = !isActive;
                Debug.Log($"Interact {isActive}");
                canvas.gameObject.SetActive(isActive);
                if (isActive) OnShopEnter();
                else OnShopExit();
                Debug.Log($"<color=green>Shop</color>: Interact User: {interactUser}");
            }  
        }

        private void Interact(InputAction.CallbackContext context)
        {
            Interact(interactUser);
        }

        protected virtual IEnumerator Initialize()
        {
            foreach (var item in _items.Where(item => item.Prefab != null))
            {
                if(item.Prefab.GetComponent<Item>() == null)
                {
                    Debug.LogError($"Prefab need to have class type of Item {item}");
                    continue;
                }
                var instance = Instantiate(prefab, parent);
                instance.item_name = item.ItemName;
                instance.item_price = item.Price;
                instance.data = item;
                instance.CurrentShop = this;
                instance.gameObject.name = item.ItemName;
                instance.Button.onClick.AddListener(() => Buy(item));
                _objects.Add(instance);
                yield return null;
            }

            canvas.gameObject.SetActive(false);
        }

        private async void Buy(ItemData data)
        {
            if (GameManager.Instance.money.Money >= data.Price)
            {
                bool response = await Popup.Instance.ShowPopup("You sure?");

                if (response)
                {
                    GameManager.Instance.money.Money -= data.Price;
                    GameManager.Instance.Player.AddItem(data);
                }

                _objects.Find(obj => obj.data == data).Button.Select();
            }
            else
            {
                Debug.LogError("No enought money");
            }
        }

        private void Start()
        {
            StartCoroutine(Initialize());
        }
    }
}
