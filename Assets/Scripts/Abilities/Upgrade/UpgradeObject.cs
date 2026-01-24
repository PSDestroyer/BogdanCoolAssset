using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeObject : Selectable
{
    [HideInInspector] public Ability ability;
    UpgradePanel _upgradePanel;
    private bool isActive;
    
    
    public Slider damage, gasUse;
    public TMP_Text cooldownValue, levelValue;
    public Button upgradeButton;

    private const string Star = "<sprite=0>";
    
    
    public bool IsActive => isActive;

    
    public void DrawUI()
    {
        if (ability == null) return;
        damage.value = ability.Damage;
        gasUse.value = ability.GasUse;
        cooldownValue.text = "Cooldown: " + ability.Cooldown.ToString("F1") + 's';
        levelValue.text = "Level:";
        for (int i = 0; i < ability.Level; i++)
        {
            levelValue.text += " " + Star;
        }
    }

    public void Activate()
    {
        isActive = true;
    }
    
    public void Initialize(UpgradePanel upgradePanel)
    {
        
        _upgradePanel = upgradePanel;

        damage.maxValue = 100f;
        gasUse.maxValue = 100f;
        
        upgradeButton.onClick.AddListener(Upgrade);
        
        // Debug.Log(gameObject.name + " Initialized " + upgradePanel.gameObject);
    }

    private void Upgrade()
    {
        ability.Upgrade();    
        DrawUI();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);
        upgradeButton.Select();
    }
    
    public override void OnDeselect(BaseEventData eventData)
    {
        base.OnDeselect(eventData);
    }
}
