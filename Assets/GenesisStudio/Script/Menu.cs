using UnityEngine.UI;
using UnityEngine;

namespace GenesisStudio
{
    public class Menu : MonoBehaviour
    {
        public Settings settings;
        public GameObject menuCanvas;
        public Selectable firstSelect;
        private GameObject activePanel;
        private bool isOpen = false;

        public bool Enabled => isOpen;

        private void Start()
        {
            menuCanvas.SetActive(false);
            settings.gameObject.SetActive(false);
        }
        public void ChangePanel(GameObject newPanel)
        {
            if(activePanel)
                activePanel.gameObject.SetActive(false);
            activePanel = newPanel;
            activePanel.gameObject.SetActive(true);
        }
        public void Toggle()
        {
            isOpen = !isOpen;
            
        }
        public void Open()
        {
            // InputManager.Instance.playerInput.actions[Needs.Cancel].performed -= GameManager.Instance.ToggleMenu;
            InputManager.Instance.ChangeMap(Needs.UIMap);
            Time.timeScale = 0;
            isOpen = true;
            menuCanvas.SetActive(true);
            if(activePanel)
                activePanel.SetActive(false);
            
            firstSelect.Select();
            
            GameEventBus.Instance.OnMenuOpened?.Invoke();
        }
        public void Close()  
        {
            InputManager.Instance.ChangeMap(Needs.PlayerMap);
            // InputManager.Instance.playerInput.actions[Needs.Cancel].performed += GameManager.Instance.ToggleMenu;
            Time.timeScale = 1;
            isOpen = false;
            menuCanvas.SetActive(false);
            if(activePanel)
                activePanel.SetActive(false);
            
            
            GameEventBus.Instance.OnMenuClosed?.Invoke();
        }

    }
    
}