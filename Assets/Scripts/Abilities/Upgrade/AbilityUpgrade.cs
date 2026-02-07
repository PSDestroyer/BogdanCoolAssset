using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;

public class AbilityUpgrade : MonoBehaviour , IInteractable
{


    public bool Hold { get; set; } = false;
    public float HoldTime { get; set; }
    public bool CanInteract { get; } = true;
    bool _isActive;

    private AbilityDatabase _abilityDatabase;
    private UpgradePanel  _upgradePanel;
    
    public void Initialize(AbilityDatabase abilityDatabase)
    {
        _abilityDatabase = abilityDatabase;
        UIManager.Instance.Find(out _upgradePanel);
    }
    
    public void Interact(object sender)
    {
        if (sender is MovementCharacterController)
        {
            _isActive = !_isActive;
            if (_isActive)
            {
                UIManager.Instance.Show(out _upgradePanel);
            }
            else
            {
                UIManager.Instance.Hide<UpgradePanel>();
            }
        }       
    }

    public void EnableAbility(Ability ability)
    {
        print(ability);
        print(_abilityDatabase);
        if (_abilityDatabase.Contains(ability.Data.ID()))
        {
            _upgradePanel.Activate(ability);
        } 
        else Debug.Log($"<color=yellow>Can't find the {ability.Data} in database </color>");
    }
}
