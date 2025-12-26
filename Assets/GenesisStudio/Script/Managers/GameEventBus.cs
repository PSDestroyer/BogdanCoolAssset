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

        public Quest quest;
        public Player player;
        
        public class Quest
        {
            //TODO: Add actions
        }

        public class Player
        {
            public Action OnMoneyChange;
            
        }
        
    }
}