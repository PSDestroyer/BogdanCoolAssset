using UnityEngine;

namespace GenesisStudio.Shooter
{
    [RequireComponent(typeof(Rigidbody), typeof(Collider))]
    public class Weapon : Item
    {
        Rigidbody _rb;
        Collider _col;
        
        [field: SerializeField] public Transform LeftHandPosition { get; private set; }
        [field: SerializeField] public Transform RightHandPosition { get; private set; }
        
        protected override void Initialize()
        {
            data.ChangeType(ItemType.Weapon);
            _rb = GetComponent<Rigidbody>();
            _col = GetComponent<Collider>();
        }

        protected override void AddItemToBrain(Brain b)
        {
            if(!b.CanAdd(this)) return;
            if (b.Contains<ShooterMotor>(out var shooterMotor))
            {
                shooterMotor.Equip(this);
                _rb.isKinematic = true;
                b.AddItem(data);
            }
        }
    }
}