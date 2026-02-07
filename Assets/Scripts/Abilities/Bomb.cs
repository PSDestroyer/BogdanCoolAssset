using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    [HideInInspector]public Rigidbody rb;
    public GameObject explosionEffect;
    public float range = 6f;
    public LayerMask damageableLayer;
    public float damagePerEnemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        var colliders = Physics.OverlapSphere(transform.position, range, damageableLayer);
        foreach (var damageable in colliders)
        {
            if(damageable.TryGetComponent(out IDamageable component))
            {
                component.ApplyDamage(damagePerEnemy);
            }
        }
        
        Destroy(gameObject);
    }
}