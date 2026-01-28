using UnityEngine;
using Newtonsoft.Json;

[CreateAssetMenu(fileName = "AbilityData", menuName = "Ability Data")]
public class AbilityData : ScriptableObject
{
    [JsonIgnore, SerializeField] private new string name, actionName;
    [JsonIgnore, SerializeField, Min(1)] private int level = 1, maxLevel = 6, upgradePrice;
    [JsonIgnore, SerializeField] private Sprite icon;
    [JsonIgnore, SerializeField] private Ability runtimePrefab;
    [JsonIgnore, SerializeField] private float cooldown;
    [JsonIgnore, SerializeField] private float gasUse, damage;
    
    
    [JsonIgnore] public int MaxLevel => maxLevel;
    [JsonIgnore] public float Damage => damage;
    [JsonIgnore] public int Level => level;
    [JsonIgnore] public Sprite Icon => icon;
    [JsonIgnore] public Ability RuntimePrefab => runtimePrefab;
    [JsonIgnore] public float Cooldown => cooldown;
    [JsonIgnore] public float GasUse => gasUse;
    [JsonIgnore] public int Price => upgradePrice;
    [JsonIgnore] public string ActionName => actionName;


    public string ID()
    {
        return name;
    }
}