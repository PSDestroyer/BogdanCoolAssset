using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Collections;
using UnityEngine;

namespace GenesisStudio
{
    public class Mission : MonoBehaviour
    {
        private QuestGameObject _activeQuest;
        private Coroutine _activeMissionCoroutine;
        private Queue<QuestInfo> _questQueue;
        
        [SerializeField] private List<QuestInfo> Quests;


        private void Awake()
        {
            _questQueue = new Queue<QuestInfo>(Quests);
        }

        public void Begin()
        {
            if(_activeMissionCoroutine == null)
                _activeMissionCoroutine = StartCoroutine(StartMission());
            GameManager.Instance.mission_currentMission = this;
        }

        public IEnumerator StartMission()
        {
            while (_questQueue.Count > 0)
            {
                var current = _questQueue.Dequeue();

                _activeQuest = QuestManager.Instance.AddQuest(current);
                if (_activeQuest.Data.IsAlreadyCompleted())
                {
                    _activeQuest.Complete();
                }
                while (!_activeQuest.IsComplete)
                {
                    yield return null; 
                }
                QuestManager.Instance.DestroyActiveQuest();
                _activeQuest = null; 
            }
            Debug.Log("Mission Completed!");
            QuestManager.Instance.CompleteAndRemoveActiveQuest();
        }

        public void Stop()
        {
            StopCoroutine(_activeMissionCoroutine);
            if(_activeQuest != null)
            {
                QuestManager.Instance.CompleteAndRemoveActiveQuest();
                _activeQuest = null;
            }
        }

        [Serializable]
        public class QuestInfo
        {
            [SerializeField] Quest questType;
            [SerializeField] QuestParams @params;

            public Quest QuestType => questType;
            public QuestParams Params => @params;
        }
    }
}
