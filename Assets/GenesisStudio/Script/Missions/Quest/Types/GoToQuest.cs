using UnityEngine;


namespace GenesisStudio
{
    [CreateAssetMenu(fileName = "Go To", menuName = "Genesis Studio/Mission/Go To Quest")]
    public class GoToQuest : Quest
    {
        Transform destination;
        GameObject indicator;
        Transform player;

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
            destination = @params.Target_point;
            indicator = destination.AddIndicator();

            if(_player is MonoBehaviour mb){
                player = mb.transform;
            }
        }

        public override void Update()
        {
            if (player.transform.IsNearThePoint(destination))
                Complete();
        }
    }
}
