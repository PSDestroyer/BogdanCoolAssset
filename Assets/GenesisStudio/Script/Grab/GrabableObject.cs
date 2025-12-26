using UnityEngine;

namespace GenesisStudio
{
    [RequireComponent(typeof(Rigidbody))]
    public class GrabableObject : MonoBehaviour, IGrabable
    {
        public void Grab(Transform hands)
        {
            transform.position = hands.position;
        }
    }
}