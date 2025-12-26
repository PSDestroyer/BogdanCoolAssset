using System.Collections;
using UnityEngine;
using GenesisStudio;

[CreateAssetMenu(fileName = "Talk to", menuName = "Genesis Studio/Mission/Talk Quest")]
public class TalkQuest : Quest
{
    NPC target;
    Dialogue speech;
    private bool _played;
    public bool DialoguePlayed 
    {
        get => _played;
        set
        {
            _played = value;
            if (_played)
            {
                Debug.Log($"Dialogue Played, Quest <color=green>{Task}</color> Completed");
                Complete();
            }
        } 
    }

    public override bool IsAlreadyCompleted()
    {
        return false;
    }

    public override void OnComplete()
    {
        DialoguePlayed = false;
    }

    public override void OnInitialize(QuestParams @params)
    {
        target = @params.Target_npc;
        speech = @params.Dialogue;
        target.Dialogue = @params.Dialogue;
    }
}
