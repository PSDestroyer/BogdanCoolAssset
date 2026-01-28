using System.Collections;
using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;

namespace Abilities
{
    public class GroundPoundAbility : Ability
    {
        [SerializeField] private float damagePerEnemy;
        [SerializeField] private float force;
        [SerializeField] private float maxRadius;
        [SerializeField] private LayerMask damageableLayer;
        
        
        protected override void Initialize()
        {
            // InputManager.Instance.Subscribe(Needs.Interact, Use);    
        }
    
        
        protected override IEnumerator Action()
        {
            if(_controller.IsGrounded) yield break;
            _controller.CanControl = false;
            while (!_controller.IsGrounded)
            {
                _controller.Motor.Move(-Vector3.up * force * Time.deltaTime);
                // _animator.SetTrigger("Airhit");
                yield return null;
            }

            float r = 0f;
            Collider[] colliders = new Collider[6];
            while (r < maxRadius)
            {
                r += force/5 * Time.deltaTime;
                colliders = Physics.OverlapSphere(_controller.LowZonePosition.position, r, damageableLayer);
            }
            foreach (var damageable in colliders)
            {
                if(damageable.TryGetComponent(out IDamageable component))
                {
                    component.ApplyDamage(damagePerEnemy);
                }
            }
            _controller.CanControl = true;
        }
    }
}