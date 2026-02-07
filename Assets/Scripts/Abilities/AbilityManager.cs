using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    [SerializeField] private AbilityDatabase _abilityDatabase;
    [SerializeField] private AbilityUpgrade _abilityUpgrade;
    [SerializeField] private Transform _abilityRoot;
    
    private List<Ability> _activeAbilities;
    private MovementCharacterController _character;
    
    private void Awake()
    {
        _character = GetComponent<MovementCharacterController>();
        _activeAbilities = new List<Ability>();
        
    }
    
    private IEnumerator Start()
    {
        _abilityUpgrade.Initialize(_abilityDatabase);
        yield return null;
        LoadAbilities();
    }

    public void LoadAbilities()
    {
        var abilities = _abilityDatabase.LoadAbilities();
        if(abilities == null) return;
        foreach (var a in abilities)
        {
            Debug.Log(a.ID());
            AddAbility(a.RuntimePrefab, true);
        }
    }

    public void AddAbility(Ability ability, bool fromSave = false)
    {
        var instance = Instantiate(ability, _abilityRoot);
        
        ability.gameObject.SetActive(fromSave);
        
        if(!_activeAbilities.Contains(instance))
            _activeAbilities.Add(instance);
        
        instance.Initialize(_character);

        _abilityUpgrade.EnableAbility(instance);
        
        GameEventBus.Instance.OnAddAbility?.Invoke(instance.Data);
        
        if (fromSave) return;
        _abilityDatabase.SaveAbility(ability);
        ability.CheckForDestroy();
    }
    
    public void AddAbility(Ability ability)
    {
        var instance = Instantiate(ability, _abilityRoot);
        
        ability.gameObject.SetActive(false);
        
        if(!_activeAbilities.Contains(instance))
            _activeAbilities.Add(instance);
        
        instance.Initialize(_character);
        instance.fromSave = false;
        
        _abilityDatabase.SaveAbility(ability);
        _abilityUpgrade.EnableAbility(instance);
        
        GameEventBus.Instance.OnAddAbility?.Invoke(instance.Data);
    }

    public List<Ability> GetActiveAbilities() => _activeAbilities;

    public void UseAbility<T>() where T : Ability
    {
        var target = _activeAbilities.FirstOrDefault(a => a.GetType() == typeof(T));
        if(target == null)
        {
            Debug.LogError("There is no ability of type: " + typeof(T).ToString());
            return;
        }
        target.Use();
    }

    public void Clear()
    {
        StartCoroutine(_activeAbilities.DestroyItemsAndClearTheListCoroutine());
    }
}
