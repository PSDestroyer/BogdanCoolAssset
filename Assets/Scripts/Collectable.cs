using System;
using GenesisStudio;
using UnityEngine;

public class Collectable : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Collect();
            Destroy(gameObject);
        }
    }
}
