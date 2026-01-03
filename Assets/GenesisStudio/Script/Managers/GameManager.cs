using System;
using GenesisStudio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SaveManager = HalvaStudio.Save.SaveManager;


public class GameManager : Singleton<GameManager>
{

    public MoneyManager money;
    [SerializeField] private GameObject player;

    public ICharacter Player => player.GetComponent<ICharacter>();
    public GameObject PlayerObject => player;
    

    public float indicator_height = 2f;
    public float indicator_arriveRange = 2f;
    public Color indicator_color = Color.yellowNice;

    

    private List<Collectable> _collectables;
    public int maxCapacity;
    public int collected;
    
    
    
    public Mission mission_currentMission { get; set; }
    
    
    protected override void AwakeInit()
    {
        if(Player == null)
            throw new Exception($"{player} does not exist or dont have ICharacter interface on it");
        
        _collectables = new List<Collectable>();
        maxCapacity = 0;
        collected = 0;

    }

    private void Start()
    {
        
    }

    public void PlayerEnable(bool value)
    {
       Player.Controls(value);  
    }

    public void Collect()
    {
        collected++;
        GameEventBus.Instance.OnItemCollected?.Invoke(collected);
    }

    public void Save()
    {
        SaveManager.Instance.Save();
    }

    public Coroutine InvokeCoroutineHelper(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

}
