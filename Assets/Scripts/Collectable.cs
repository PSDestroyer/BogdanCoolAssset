using System;
using GenesisStudio;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    private void Start()
    {
        GameManager.Instance.AddCollectable(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.collected++;
            GameEventBus.Instance.OnItemCollected?.Invoke();
            Destroy(gameObject);
        }
    }
}
