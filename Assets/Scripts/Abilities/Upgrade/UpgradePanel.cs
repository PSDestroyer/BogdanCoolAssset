using System.Collections;
using System.Collections.Generic;
using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;

public class UpgradePanel : UIScreen
{
    public GameObject upgradeCamera;
    public List<UpgradeObject> upgrades;
    [HideInInspector] public UpgradeObject selected;

    public MovementCharacterController player;
    
    private CanvasGroup _canvasGroup;
    private int _emptyUpgrade = 0;
    
    public void Activate(Ability a)
    {
        if(_emptyUpgrade >= upgrades.Count)
        {
            Debug.LogError("There is no upgrades with index " + (_emptyUpgrade + 1));
            return;
        }
        
        var target = upgrades[_emptyUpgrade];
        
        target.ability = a;
        target.Activate();
        if(_emptyUpgrade != upgrades.Count)
            _emptyUpgrade++;
    }

    protected override IEnumerator OnShow()
    {
        player.Controls(false);
        foreach (var uo in upgrades)
        {
            uo.interactable = uo.IsActive;
            if(uo.IsActive) uo.DrawUI();
            uo.gameObject.SetActive(uo.IsActive);
        }
        upgrades[0].upgradeButton.Select();
        upgradeCamera.SetActive(true);
        // _uiManager.Hide<HUDScreen>();
        yield return _canvasGroup.Fade(0, 1, .5f);
                
    }

    protected override IEnumerator OnHide()
    {
        yield return _canvasGroup.Fade(1, 0, .5f);
        foreach (var uo in upgrades) 
            uo.interactable = false;
        upgradeCamera.SetActive(false);
        player.Controls(true);
        // _uiManager.Show<HUDScreen>(out _);
        // player.PlayerAnimator.gameObject.SetActive(true); 
    }

    public override void Initialize()
    {
        selected = null;
        _emptyUpgrade = 0;
        foreach (var uo in upgrades)
        {
            uo.Initialize(this);
        }

        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0;
    }
}
