using System;

namespace GenesisStudio
{
    public class GameEventBus : Singleton<GameEventBus>
    {

        public Action<Quest, QuestParams> OnQuestAdded;
        public Action<QuestGameObject> OnQuestCompleted;

        public Action OnMenuOpened;
        public Action OnMenuClosed;


        //Player

        public Action<ICharacter, ItemData> OnItemAdded;
        public Action<ICharacter, ItemData> OnItemRemoved;

    }
}