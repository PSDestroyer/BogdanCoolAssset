using System.Collections;
using System.Collections.Generic;
using HalvaStudio.Save;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenesisStudio
{
    public class UIManager : Singleton<UIManager>
    {
        [Space(10f)]
        [Header("Quest")]
        [SerializeField] private Transform quest_Parent;
        [SerializeField] private QuestGameObject quest_Prefab;
        private QuestGameObject _activeQuest;

        [Header("Screens")]
        [SerializeField] List<UIScreen> Screens = new List<UIScreen>();

        [Header("Notification")]
        [SerializeField] private TMP_Text TMP_message;
        [SerializeField] private Animator _notification;
        [SerializeField] private string animationName;
        private Queue<string> _notifications = new Queue<string>();
        private string _message
        {
            get => TMP_message.text;
            set => TMP_message.text = value;
        }



        [Header("Crosshair")]
        [SerializeField] private Slider HoldingCrosshair;
        public float HoldingValue
        {
            set
            {
                HoldingCrosshair.value = value;
            }
        }



        UIScreen _currentScreen;


        public void Show<T>(out T screen) where T : UIScreen
        {
            screen = null;
            if(_currentScreen != null)
                _currentScreen.Hide();

            _currentScreen = Find<T>();

            if(_currentScreen == null)
                throw new System.Exception($"<color=green>UIManager</color>: There is no type of {typeof(T)} Screen");

            _currentScreen.Show();
            screen = _currentScreen as T;
        }

        public void Find<T>(out T screen) where T : UIScreen
        {
            screen = Screens.Find(s => s.GetType() == typeof(T)) as T;
        }
        public T Find<T>() where T : UIScreen
        {
            return Screens.Find(s => s.GetType() == typeof(T)) as T;
        }

        public void HideCurrentScreen()
        {
            if(_currentScreen != null)
                _currentScreen.Hide();
        }

        private void Start()
        {
            GameEventBus.Instance.OnQuestAdded += InitializeQuest;
            
            foreach (var s in Screens)
            {
                s.Initialize();
                if(s.HideOnStart) s.Hide();
            }
            
            GameEventBus.Instance.OnItemCollected?.Invoke(SaveManager.Instance.saveData.collected);
            GameEventBus.Instance.OnGasChanged?.Invoke(SaveManager.Instance.saveData.gas);
            GameEventBus.Instance.OnHealthChanged?.Invoke(SaveManager.Instance.saveData.health);
        }

        

        public void Settings()
        {
            Show<Settings>(out _);
        }

        public void Menu()
        {
            Show<Menu>(out _);
        }

        public void InitializeQuest(Quest type, QuestParams data)
        {
            _activeQuest = Instantiate(quest_Prefab, quest_Parent);
            _activeQuest.Initialize(type, data);
        }



        public void ShowNotification(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Debug.LogError($"<color=green>Norification Manager</color>: The notification is empty");
                return;
            }
            _message = message;
            _notification.Play(animationName);
        }

        private IEnumerator notify(string message)
        {
            _notifications.Enqueue(message);
            while (_notifications.Count > 0)
            {
                _message = _notifications.Dequeue();
                _notification.Play(animationName);

                yield return null;
            }
        }
    
        
    }
}