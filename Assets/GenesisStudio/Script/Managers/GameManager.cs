using GenesisStudio;
using System.Collections;
using UnityEngine;
using Outline = GenesisStudio.Outline;
using SaveManager = HalvaStudio.Save.SaveManager;


public class GameManager : Singleton<GameManager>
{
    
    public MoneyManager money;
    [SerializeField] private Player player;

    bool player_Enabled = true;

    public Player Player => player;
    
    [field: SerializeField] public Mission mission_currentMission { get; set; }

    public float indicator_height = 2f;
    public float indicator_arriveRange = 2f;
    public Color indicator_color = Color.yellowNice;

    private Outline currentOutline;
    [field: SerializeField] public OutlineManager OutlineManager { get; private set; }
    
    protected override void AwakeInit()
    {
    }
    
    public void PlayerEnable(bool value)
    {
        // player_Enabled = value;
        // player.Controls(value);  
        // UI_MainPanel.SetActive(value);
    }


    public void AddMoney(int amount)
    {
        money.Money += amount;
    }

    public void RemoveMoney(int amount)
    {
        money.Money -= amount;
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
