using System;
using System.Collections.Generic;
using UnityEngine;

public class BuffManager : MonoBehaviour
{
    private List<Buff> _activeBuffs;
    private ICharacter _character;
    
    
    
    private void Awake()
    {
        _character = GetComponent<ICharacter>();
        _activeBuffs = new List<Buff>();
    }
    
    public void AddBuff(Buff buff)
    {
        if(!_activeBuffs.Contains(buff))
            _activeBuffs.Add(buff);
        
        buff.Initialize();
    }
    
        
}
