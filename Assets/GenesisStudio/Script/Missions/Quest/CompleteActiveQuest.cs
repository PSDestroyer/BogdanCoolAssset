using GenesisStudio;
using UnityEngine;
using UnityEngine.Events;

public class CompleteActiveQuest : MonoBehaviour, IInteractable
{
    public UnityEvent onComplete;
    public bool CanInteract => true;

    [field: SerializeField] public bool Hold { get; set; }
    [field: SerializeField] public float HoldTime { get; set; }

    public void Interact(object sender)
    {
        Complete();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
           Complete();
        }
    }

    public void Complete()
    {
        QuestManager.Instance.CompleteAndRemoveActiveQuest();
        onComplete?.Invoke();
        Destroy(this);
    }
}
