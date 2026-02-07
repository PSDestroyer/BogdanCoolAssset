using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GenesisStudio;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using PlatformCharacterController;
using Unity.VisualScripting;

public class HUDScreen : UIScreen
{
    [SerializeField] private Slider healthBar, gasBar;
    [SerializeField] private TMP_Text collected;
    [SerializeField] private MovementCharacterController _player;

    [Serializable]
    private class AbilityUI
    {
        public AbilityData data;
        public Image Image;
        [HideInInspector] public CanvasGroup cg_image;

        public void Initialize()
        {
            if(data.Icon != null)
                Image.sprite = data.Icon;
            else throw new NullReferenceException($"There is no icon for this ability {data}");
            
            Image.TryGetComponent(out cg_image);
            
            if(cg_image == null)
                cg_image = Image.AddComponent<CanvasGroup>();
            
            cg_image.alpha = 0.1f;
        }
        public void Activate()
        {
            cg_image.alpha = 1f;
        }
    }
    
    [Header("Abilities")]
    [SerializeField] private List<AbilityUI> _abilities;
    
    private AbilityManager AbilityManager => _player.AbilityManager;
    
    
    protected override IEnumerator OnShow()
    {
        yield return StartCoroutine(_canvasGroup.Fade(0, 1, 0.2f));
    }

    protected override IEnumerator OnHide()
    {
        yield return StartCoroutine(_canvasGroup.Fade(1, 0, 0.2f));
    }

    private void Cooldown(AbilityData data)
    {
        var target = _abilities.FirstOrDefault(a => a.data == data);
        if (target != null)
        {
            StartCoroutine(Cooldown(data.Cooldown, target));
        }
    }
    
    private IEnumerator Cooldown(float time, AbilityUI target)
    {
        target.cg_image.alpha = 0.1f;
        yield return new WaitForSeconds(time - .2f);
        yield return StartCoroutine(target.cg_image.Fade(0.1f,1, .2f));
    }

    private void OnAddAbility(AbilityData data)
    {
        var target = _abilities.FirstOrDefault(a => a.data == data);
        if(target != null)
            target.Activate();
        else Debug.Log($"<color=yellow>There is no this type of {data}</color>");
    }
    
    
    public override void Initialize()
    {
        _hideOnStart = false;
        
        GameEventBus.Instance.OnGasChanged += OnGasChanged;
        GameEventBus.Instance.OnItemCollected += OnItemCollected;
        GameEventBus.Instance.OnHealthChanged += OnHealthChanged;
        GameEventBus.Instance.OnUseAbility += Cooldown;
        GameEventBus.Instance.OnAddAbility += OnAddAbility;
        
        foreach (var abilityUI in _abilities)
        {
            abilityUI.Initialize();
        }
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
