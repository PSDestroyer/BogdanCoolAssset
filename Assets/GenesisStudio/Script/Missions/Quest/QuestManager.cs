using UnityEngine;
namespace GenesisStudio
{
    public class QuestManager : Singleton<QuestManager>
    {
        private QuestGameObject _activeQuestGO;
        private Quest _activeQuest;

        public QuestGameObject ActiveQuestGO { get => _activeQuestGO; }
        public Quest ActiveQuest { get => _activeQuest; }
        public bool IsActiveQuest => _activeQuest != null;

        public Quest AddQuest(Mission.QuestInfo data)
        {
            //var temp = new GameObject($"{(data.Params.Task)}");
            //_activeQuestGO = temp.AddComponent<QuestGameObject>();
            //_activeQuestGO.Initialize(data.QuestType, data.Params);
            
            data.Params.Player = GameManager.Instance.Player;

            _activeQuest = data.QuestType;
            _activeQuest.Initialize(data.Params);
            return _activeQuest;
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
            if(IsActiveQuest)
            {
                quest = ActiveQuest;
                return true;
            }
            return false;
        }
        public void CompleteCurrentQuest()
        {
            if (!IsActiveQuest) return;
            _activeQuest.Complete();
            
        }
    }
}