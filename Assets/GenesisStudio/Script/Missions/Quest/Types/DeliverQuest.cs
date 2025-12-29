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
        NPC DeliverTarget;

        public override bool IsAlreadyCompleted()
        {
            return false;
        }

        public override void OnComplete()
        {
            DeliverTarget = null;
            DeliverItem = null;
            GameEventBus.Instance.OnItemAdded -= OnItemDelivered;
        }

        public override void OnInitialize(QuestParams @params)
        {
            DeliverTarget = @params.Target_npc;
            DeliverItem = @params.Target_item;
            GameEventBus.Instance.OnItemAdded += OnItemDelivered;
        }

        public void OnItemDelivered(ICharacter character, ItemData item)
        {
            Debug.Log($"Check For Quest {character}");

            if (character is NPC target && target == DeliverTarget)
            {
                if (item == DeliverItem)
                        Complete();
            }
        }
    }
}