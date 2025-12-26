using System.Collections.Generic;
using UnityEngine;

namespace GenesisStudio
{
    public class AIInteractManager : Singleton<AIInteractManager>
    {
        private List<AIInteractable> _interactables = new List<AIInteractable>();
        
        public void RegisterInteractable(AIInteractable toRegister, AIBrain user)
        {
            Debug.Log($"<color=green>AIInteractManager</color>: Register {toRegister} brain {user}");
            _interactables.Add(toRegister);
        }
   
        public void DeregisterInteractable(AIInteractable toDeregsiter, AIBrain user)
        {
            _interactables.Remove(toDeregsiter);
            Debug.Log($"<color=green>AIInteractManager</color>: Deregister {toDeregsiter} brain {user}");
        }
    }

    public enum EInteractType
    {
        None,
        Door,
        AnotherAI,
        Object
    }
}