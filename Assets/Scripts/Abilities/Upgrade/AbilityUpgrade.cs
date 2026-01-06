using System.Collections.Generic;
using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;

public class AbilityUpgrade : MonoBehaviour , IInteractable
{
    public GameObject upgradeCamera;

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
        if (sender is MovementCharacterController player)
        {
            _isActive = !_isActive;
            if (_isActive)
            {
                UIManager.Instance.Show(out _upgradePanel);
                _upgradePanel.player = player;
                upgradeCamera.SetActive(true);
            }
            else
            {
                UIManager.Instance.HideCurrentScreen();
                player.PlayerAnimator.gameObject.SetActive(true); 
                upgradeCamera.SetActive(false);
                player.Controls(true);
            }
        }       
    }

    public void EnableAbility(Ability ability)
    {
        if (_abilityDatabase.Contains(ability.Data.ID()))
        {
            _upgradePanel.Activate(ability);
        }
    }
}
