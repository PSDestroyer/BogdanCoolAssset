using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
namespace GenesisStudio
{
    [CreateAssetMenu(fileName = "Deliver", menuName = "Genesis Studio/Mission/Deliver Quest")]
    public class DeliverQuest : Quest
    {
        ItemData DeliverItem;
        int DeliverAmount;
        NPC DeliverTarget;
        int Delivered;

        public override bool IsAlreadyCompleted()
        {
            return false;
        }

        public override void OnComplete()
        {
            DeliverTarget = null;
            DeliverItem = null;
            DeliverAmount = 0;
            Delivered = 0;
        }

        public override void OnInitialize(QuestParams @params)
        {
            DeliverTarget = @params.Target_npc;
            DeliverItem = @params.Target_item;
            _player.SetTargetItem(@params.Target_item);
            DeliverAmount = @params.Amount;
            Delivered = 0; 
        }

        public void OnItemDelivered(ItemData item)
        {
            if (item == DeliverItem)
                Complete();
        }

    }
}