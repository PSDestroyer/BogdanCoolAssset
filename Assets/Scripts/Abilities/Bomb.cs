using System;
using Abilities;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bomb : MonoBehaviour
{
    public Rigidbody rb;
    public GameObject explosionEffect;
    public float range = 6f;
    public LayerMask enemyLayer;
    public float damagePerEnemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        GetComponent<MeshRenderer>().enabled = false;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        
        var enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);
        foreach (var obj in enemies)
        {
            if(obj.TryGetComponent(out Enemy enemy))
                enemy.Health -= damagePerEnemy;
        }
        
        Destroy(gameObject);
    }
}