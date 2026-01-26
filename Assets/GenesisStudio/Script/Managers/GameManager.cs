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
    public Portal portal;
    public LevelData nextLevel;
    public Boss boss;
    public List<Enemy> enemies;
    public ICharacter Player => player.GetComponent<ICharacter>();
    public GameObject PlayerObject => player;
    
    
    public float indicator_height = 2f;
    public float indicator_arriveRange = 2f;
    public Color indicator_color = Color.yellowNice;
    
    

    private List<Collectable> _collectables;
    public int collected;


    public Mission mission_currentMission { get; set; }


    protected override void AwakeInit()
    {
        if (Player == null)
            throw new Exception($"{player} does not exist or dont have ICharacter interface on it");

        _collectables = new List<Collectable>();
        collected = 0;

    }

    private void Start()
    {
        portal.Deactivate();
        GameEventBus.Instance.OnEnemyDie += OnEnemyDie;
    }

    public void Complete()
    {
        UIManager.Instance.Show<EndOfLevelScreen>(out _);
        
    }
    
    public void PlayerEnable(bool value)
    {
       Player.Controls(value);  
    }

    public void Collect()
    {
        SaveManager.Instance.saveData.collected++;
        GameEventBus.Instance.OnItemCollected?.Invoke(collected);
    }

    [ContextMenu(nameof(Save))]
    public void Save()
    {
        SaveManager.Instance.Save();
    }

    private void OpenPortal()
    {
        portal.Activate();
    }

    private void OnEnemyDie(Enemy enemy)
    {
        if (enemies.Count <= 0) return;
        
        if (enemies.Contains(enemy))
            enemies.Remove(enemy);

        if(enemies.Count == 0)
            OpenPortal();
    }

    
    public Coroutine InvokeCoroutineHelper(IEnumerator coroutine)
    {
        return StartCoroutine(coroutine);
    }

    public void Restart()
    {
        //get checkpoint, load data from checkpoint
    }
}
