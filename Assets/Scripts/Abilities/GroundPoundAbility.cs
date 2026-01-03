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
        [SerializeField] private LayerMask enemyLayer;
        
        
        protected override void Initialize()
        {
            InputManager.Instance.Subscribe(Needs.Interact, Use);    
        }
    
        
        protected override IEnumerator C_Use()
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
            Collider[] enemies = new Collider[6];
            while (r < maxRadius)
            {
                r += force/5 * Time.deltaTime;
                enemies = Physics.OverlapSphere(_controller.LowZonePosition.position, r, enemyLayer);
            }
            foreach (var enemy in enemies)
            {
                enemy.GetComponent<Enemy>().Health -= damagePerEnemy;
            }
            _controller.CanControl = true;
        }
    }
}