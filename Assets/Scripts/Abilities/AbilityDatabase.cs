using System.Collections.Generic;
using System.Linq;
using HalvaStudio.Save;
using UnityEngine;

[CreateAssetMenu(fileName = "AbilityDatabase", menuName = "Ability Database")]
public class AbilityDatabase : ScriptableObject
{
    [SerializeField] private List<AbilityData> abilities;

    public AbilityData Get(string ID)
    {
        return abilities.FirstOrDefault(aData => aData.ID() == ID);
    }
    
    public AbilityData Get(Ability ability)
    {
        return abilities.FirstOrDefault(aData => aData.ID() == ability.Data.ID());
    }
    
    public Ability Get<T>() where T : Ability
    {
        Ability result = abilities.FirstOrDefault(aData => aData.RuntimePrefab.GetType() == typeof(T))!.RuntimePrefab;
        if (result == null)
        {
            Debug.LogError($"There is no such as {typeof(T)} ability in database");
            return null;
        }
        
        return result;
    }

    public void SaveAbility(Ability ability)
    {
        SaveManager.Instance.saveData.SaveAbility(new SaveManager.SaveData.AbilitySaveData(ability.Data));
    }

    public bool Contains(string ID)
    {
        return abilities.Any(aData => aData.ID() == ID);
    }

    public bool Contains(AbilityData ability)
    {
        return abilities.Contains(ability);
    }

    public List<AbilityData> LoadAbilities()
    {
        var result = new List<AbilityData>();
        var abilities = SaveManager.Instance.saveData.abilities;
        
        if(abilities == null) return null;
        if (abilities.Count < 0)
        {
            Debug.LogError("There are no abilities in database");
            return null;
        }
        
        foreach (var abilitySaveData in abilities)
        {
            AbilityData target = Get(abilitySaveData.id);
            
            if(target == null)
            {
                Debug.LogError("There is no ability with id: " +  abilitySaveData.id);
                continue;
            }
            
            result.Add(target);
        }
        return result;
    }
            
    
}