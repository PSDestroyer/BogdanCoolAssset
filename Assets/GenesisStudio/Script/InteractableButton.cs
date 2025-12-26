using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace GenesisStudio
{
    public class InteractableButton : MonoBehaviour, IInteractable
    {
        public UnityEvent onInteract;
        public int delay;
        public bool CanInteract { get; set; } = true;
        [field: SerializeField] public bool Hold { get; set; }
        [field: SerializeField] public float HoldTime { get; set; }

        public virtual async void Interact(object sender)
        {
            onInteract?.Invoke();
            await Task.Delay(delay * 1000);
        }
    }
}