using System;
using System.Collections;
using GenesisStudio;
using HalvaStudio.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndOfLevelScreen : UIScreen
{
    [SerializeField] private TMP_Text _missionStatus, _collected;
    [SerializeField] private Button _home, _next, _restart;

    private LevelData _nextLevel;
    
    string collected => SaveManager.Instance.saveData.collected + "<sprite=0>";
    
    protected override IEnumerator OnShow()
    {
        _collected.text = collected;
        yield return _canvasGroup.Fade(0f,1f, .4f);
        yield return null;
    }

    protected override IEnumerator OnHide()
    {
        _canvasGroup.alpha = 0f;
        yield return null;
    }

    public override void Initialize()
    {
        _canvasGroup.alpha = 0f;
        _hideOnStart = true;
        _nextLevel = GameManager.Instance.nextLevel;
        _next.gameObject.SetActive(!_nextLevel.IsFinal);
        
        _home.onClick.AddListener(() => LevelManager.Instance.Menu());
        _restart.onClick.AddListener(() => GameManager.Instance.Restart());
        _next.onClick.AddListener(() => Next());
    }

    private void Next()
    {
        LevelManager.Instance.LoadScene(_nextLevel);
    }
    
    
}