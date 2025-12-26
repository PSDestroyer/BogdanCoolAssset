using System.Collections;
using UnityEngine;
using GenesisStudio;
using System;

[CreateAssetMenu(fileName = "Interact With", menuName = "Genesis Studio/Mission/Interact With Quest")]
public class InteractWithQuest : Quest
{
    IInteractable interactable;

    public override bool IsAlreadyCompleted()
    {
        return false;
    }

    public override void OnComplete()
    {
       
    }

    public override void OnInitialize(QuestParams @params)
    {
        if(@params.Target_gameObject.TryGetComponent(out IInteractable interactable))
        {
            this.interactable = interactable; 
        } else
        {
            throw new NullReferenceException($"Traget gameObject is null or dont have interactable script on it{@params.Target_gameObject}");
        }
    }

    public void CheckForComplete(IInteractable interact)
    {
        if (interactable == interact)
            Complete();
    }
}
