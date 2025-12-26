using UnityEngine;
namespace GenesisStudio
{
    public class QuestManager : Singleton<QuestManager>
    {
        [SerializeField] private QuestGameObject quest_Prefab;
        [SerializeField] private Transform quest_Parent;
        private QuestGameObject _activeQuestGO;
        public QuestGameObject ActiveQuestGO { get => _activeQuestGO; }
        public Quest ActiveQuest { get => _activeQuestGO.Data; }
        public bool IsActiveQuest => _activeQuestGO != null;

        public QuestGameObject AddQuest(Mission.QuestInfo data)
        {
            _activeQuestGO = Instantiate(quest_Prefab, quest_Parent);
            _activeQuestGO.Initialize(data.QuestType, data.Params);
            data.Params.QuestGameObject = _activeQuestGO;
            data.Params.Player = GameManager.Instance.Player;
            data.QuestType.Initialize(data.Params);
            GameEventBus.Instance.OnQuestAdded?.Invoke(_activeQuestGO);
            return _activeQuestGO;
        }

        public void CompleteAndRemoveActiveQuest()
        {
            CompleteCurrentQuest();
            DestroyActiveQuest();
        }

        private void Update()
        {
            if(_activeQuestGO != null) 
                ActiveQuest?.Update();
        }

        public void DestroyActiveQuest()
        {
            if (_activeQuestGO == null) return;
            Destroy(_activeQuestGO.gameObject, 1f);
            _activeQuestGO = null;
        }

        public bool HasActiveQuest(out Quest quest)
        {
            quest = null;
            if(_activeQuestGO != null)
            {
                quest = ActiveQuest;
                return true;
            }
            return false;
        }
        public void CompleteCurrentQuest()
        {
            if (_activeQuestGO == null) return;
            _activeQuestGO.Complete();
            GameEventBus.Instance.OnQuestCompleted?.Invoke(_activeQuestGO);
        }
    }
}