using System;
using PlatformCharacterController;
using UnityEngine;

public class PlayerObstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovementCharacterController player))
        {
            player.Health = -1;
        }
    }
}
