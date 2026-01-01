using System;
using System.Collections.Generic;
using System.Linq;
using PlatformCharacterController;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
    private List<Ability> _activeAbilities;
    private MovementCharacterController _character;
    
    private void Awake()
    {
        _character = GetComponent<MovementCharacterController>();
        _activeAbilities = new List<Ability>();
    }
    
    public void AddAbility(Ability ability)
    {
        var instance = Instantiate(ability, _character.transform);
        instance.transform.position = new Vector3(0, -10, 0);
        DontDestroyOnLoad(instance);
        ability.gameObject.SetActive(false);
        if(!_activeAbilities.Contains(instance))
            _activeAbilities.Add(instance);
        
        instance.Initialize(_character);
    }

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
        
}
