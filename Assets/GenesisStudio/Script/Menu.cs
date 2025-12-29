using UnityEngine.UI;
using UnityEngine;

namespace GenesisStudio
{
    public class Menu : UIScreen
    {
        public Selectable firstSelect;
        private bool isOpen = false;

        public void Toggle()
        {
            isOpen = !isOpen;
            
        }
        public override void OnShow()
        {
            // InputManager.Instance.playerInput.actions[Needs.Cancel].performed -= GameManager.Instance.ToggleMenu;
            InputManager.Instance.ChangeMap(Needs.UIMap);
            isOpen = true;
            
            firstSelect.Select();
            
            GameEventBus.Instance.OnMenuOpened?.Invoke();
        }
        public override void OnHide()  
        {
            InputManager.Instance.ChangeMap(Needs.PlayerMap);
            // InputManager.Instance.playerInput.actions[Needs.Cancel].performed += GameManager.Instance.ToggleMenu;
            isOpen = false;
            
            GameEventBus.Instance.OnMenuClosed?.Invoke();
        }

    }
    
}