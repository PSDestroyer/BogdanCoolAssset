using System;

namespace GenesisStudio
{
    public class GameEventBus
    {
        private static GameEventBus _instance;
        public static GameEventBus Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new GameEventBus();
                
                return _instance;
            }
        }

        public Action<QuestGameObject> OnQuestAdded;
        public Action<QuestGameObject> OnQuestCompleted;

        public Action OnMenuOpened;
        public Action OnMenuClosed;

    }
}