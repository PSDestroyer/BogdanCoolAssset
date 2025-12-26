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

        public Action OnQuestAdded;
        public Action OnQuestCompleted;

        public Action OnMenuOpened;
        public Action OnMenuClosed;

    }
}