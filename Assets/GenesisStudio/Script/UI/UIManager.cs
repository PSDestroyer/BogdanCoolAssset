using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenesisStudio
{
    public class UIManager : Singleton<UIManager>
    {
        [Header("Player")] 
        [SerializeField] private Slider healthSlider;
        [SerializeField] private TMP_Text collected, needToCollect;
        
                
        
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





        public void Show<T>() where T : UIScreen
        {
            if(_currentScreen != null)
                _currentScreen.Hide();

            _currentScreen = Screens.Find(s => s.GetType() == typeof(T));

            if(_currentScreen == null)
                throw new System.Exception($"<color=green>Norification Manager</color>: There is no type of {typeof(T)} Screen");

            _currentScreen.Show();
        }

        private void Start()
        {
            GameEventBus.Instance.OnQuestAdded += InitializeQuest;
            GameEventBus.Instance.OnHealthChanged += OnHealthChanged;
            GameEventBus.Instance.OnItemCollected += OnItemCollected;


            Screens.ForEach(s => s.Hide());
        }

        private void OnItemCollected(int collected, int needToCollect)
        {
            this.collected.text = collected.ToString();
            this.needToCollect.text = needToCollect.ToString();
        }

        public void Settings()
        {
            Show<Settings>();
        }

        public void Menu()
        {
            Show<Menu>();
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
    
        public void OnHealthChanged(float value)
        {
            healthSlider.value = value;
        }
    }
}