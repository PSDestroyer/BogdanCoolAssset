using System;
using System.Transactions;
using UnityEngine;

namespace GenesisStudio
{
    public interface IInteractable
    {
        public bool CanInteract { get; } 
        public bool Hold { get; set; }
        public float HoldTime { get; set; }
        public void Interact(object sender);
    }
}