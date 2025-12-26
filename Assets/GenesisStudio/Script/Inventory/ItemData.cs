using UnityEngine;


[CreateAssetMenu(menuName = "Genesis Studio/Inventory/Item Data", fileName = "New Item Data")]
public class ItemData : ScriptableObject
{
    [SerializeField] private ItemType type;
    [SerializeField] private string itemName;
    [SerializeField] private Sprite icon;
    [SerializeField] private bool isStackable;
    [SerializeField] private bool isUsable;
    [SerializeField] private bool removeAfterUse;
    [SerializeField] private int maxStackSize;
    [SerializeField] private int price;
    [SerializeField] private Color outlineColor = Color.white;
    [SerializeField] private int healAmount;
    [SerializeField] private GameObject prefab;
    [SerializeField, TextArea] private string itemDescription;


    //Proprieties 
    public string ItemName => string.IsNullOrEmpty(itemName) ? name : itemName;
    public int Price => price;
    public GameObject Prefab => prefab;
    public Sprite Icon => icon;
    public string ItemID => itemName;
    public bool IsStackable => isStackable;
    public int MaxStackSize => maxStackSize;
    public ItemType ItemType => type;
    public string ItemDescription => itemDescription;
    public int HealAmount => healAmount; 
    public bool IsUsable => isUsable;
    public bool RemoveAfterUse => removeAfterUse;
    public Color OutlineColor => outlineColor;

    public void ChangeType(ItemType newType)
    {
        type = newType;
    }

}