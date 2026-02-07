using System.Collections;
using PlatformCharacterController;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenesisStudio
{
    public class Menu : UIScreen
    {
        private MovementCharacterController _player;
        
        protected override IEnumerator OnShow()
        {
            InputManager.Instance.ChangeMap(Needs.UIMap);
            _player.CanControl = false;
            GameEventBus.Instance.OnMenuOpened?.Invoke();
            yield return StartCoroutine(_canvasGroup.Fade(0,1,0.1f));
        }

        protected override IEnumerator OnHide()
        {
            InputManager.Instance.ChangeMap(Needs.PlayerMap);
            _player.CanControl = true;
            GameEventBus.Instance.OnMenuClosed?.Invoke();
            yield return StartCoroutine(_canvasGroup.Fade(1,0,0.1f));
        }

        
        
        public override void Initialize()
        {
            
            _player = GameManager.Instance.Player as MovementCharacterController;
        }
    }
    
}