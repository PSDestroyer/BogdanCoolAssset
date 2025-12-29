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
                return true;
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
            if (sender is ICharacter player)
            {
                Debug.Log($"{player}");
                player.GiveItem(this, player.Inventory().GetSelectedItem.Data);
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
            }
        }
    }
}
