using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace PlatformCharacterController
{
    public class JumpingPlatform : MonoBehaviour
    {
        [Tooltip("This is the jumping forze of this plataform")]
        public float JumpForze = 4;

        [HideIfNoComponent(typeof(Animator))] public Animator PlatformAnimator;

        private void OnTriggerStay(Collider other)
        {
            if (!other.CompareTag("Player")) return;
            //make the player jump
            other.GetComponent<MovementCharacterController>().Jump(JumpForze);
            //animate platform if exist animator
            if (PlatformAnimator)
            {
                PlatformAnimator.SetTrigger("In");
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            
            Gizmos.DrawSphere(transform.position + Vector3.up * JumpForze, 2f);
        }
    }
}