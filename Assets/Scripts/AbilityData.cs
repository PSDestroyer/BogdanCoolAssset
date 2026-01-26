using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Ability Data")]
public class AbilityData : ScriptableObject
{
    [SerializeField] private new string name, actionName;
    [SerializeField, Min(1)] private int level = 1, maxLevel = 6, upgradePrice;
    [SerializeField] private Sprite icon;
    [SerializeField] private Ability runtimePrefab;
    [SerializeField] private float cooldown;
    [SerializeField] private float gasUse, damage;
    
    
    public int MaxLevel => maxLevel;
    public float Damage => damage;
    public int Level => level;
    public Sprite Icon => icon;
    public Ability RuntimePrefab => runtimePrefab;
    public float Cooldown => cooldown;
    public float GasUse => gasUse;
    public int Price => upgradePrice;

    public string ActionName => actionName;


    public string ID()
    {
        return name;
    }
}