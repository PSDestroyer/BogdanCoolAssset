using System.Collections;
using GenesisStudio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDScreen : UIScreen
{
    [SerializeField] private Slider healthBar, gasBar;
    [SerializeField] private TMP_Text collected;
    
    protected override IEnumerator OnShow()
    {
        throw new System.NotImplementedException();
    }

    protected override IEnumerator OnHide()
    {
        throw new System.NotImplementedException();
    }

    public override void Initialize()
    {
        _hideOnStart = false;
        
        GameEventBus.Instance.OnGasChanged += OnGasChanged;
        GameEventBus.Instance.OnItemCollected += OnItemCollected;
        GameEventBus.Instance.OnHealthChanged += OnHealthChanged;
        
    }
    
    private void OnGasChanged(float val)
    {
        gasBar.value = val;
    }

    private void OnItemCollected(int collected)
    {
        this.collected.text = collected.ToString();
    }

    private void OnHealthChanged(float value)
    {
        healthBar.value = value;
    }
}
