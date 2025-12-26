using System;
using UnityEngine;
using UnityEngine.Events;

namespace GenesisStudio
{
    public class NPC : AIBrain, IInteractable
    {
        private bool _dialogueWasPlayed;

        #region Proprieties
        public Dialogue Dialogue { get; set; }

        public bool CanInteract
        {
            get
            {
                bool quest = false;
                quest = QuestManager.Instance.IsActiveQuest ? (QuestManager.Instance.ActiveQuestGO.DataParams.Target_npc == this) : false;
                return quest || Dialogue != null;
            }
        }
        public bool Hold { get; set; }
        public float HoldTime { get; set; } = 2f;

        #endregion

        protected override void Initialize()
        {
            base.Initialize();
        }

        public virtual void Interact(object sender)
        {
            if (sender is Player player)
            {
                if (Dialogue != null)
                {
                    DialogueManager.Instance.StartDialogue(Dialogue);
                    if (QuestManager.Instance.HasActiveQuest(out var Talkq))
                    {
                        if (Talkq is TalkQuest tq)
                        {
                            tq.DialoguePlayed = true;
                        }
                    }
                }

                if (QuestManager.Instance.HasActiveQuest(out var Deliverq))
                {
                    if (player.TryGiveTargetItem(this, out var targetItem))
                    {
                        if (Deliverq is DeliverQuest dq)
                        {
                            dq.OnItemDelivered(targetItem);
                        }
                    }
                }
            }
        }
    }
}