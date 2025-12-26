using UnityEngine;


namespace GenesisStudio
{
    [CreateAssetMenu(fileName = "Go To", menuName = "Genesis Studio/Mission/Go To Quest")]
    public class GoToQuest : Quest
    {
        Player player;
        Transform destination;
        GameObject indicator;

        public override bool IsAlreadyCompleted()
        {
            return player.transform.IsNearThePoint(destination);
        }

        public override void OnComplete()
        {
            Destroy(indicator);
        }

        public override void OnInitialize(QuestParams @params)
        {
            player = @params.Player;
            destination = @params.Target_point;
            indicator = destination.AddIndicator();
        }

        public override void Update()
        {
            if (player.transform.IsNearThePoint(destination))
                Complete();
        }
    }
}
