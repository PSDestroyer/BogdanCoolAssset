using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class CollectableContainer : Damageable
{
    [SerializeField] private int minAmount, maxAmount;
    [SerializeField] Collectable prefab;
    [SerializeField] Transform[] spawnPoints;
    
    int RandomRange => Random.Range(minAmount, maxAmount);

    
    public override void ApplyDamage(float damage)
    {
        if(_health <= 0)
            return;
        base.ApplyDamage(damage);
        StartCoroutine(SpawnCollectables());
    }

    private IEnumerator SpawnCollectables()
    {
        int count = RandomRange;
        for (int i = 0; i < count; i++)
        {
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            var instance = Instantiate(prefab, randomPoint.position, randomPoint.rotation);
            instance.Rigidbody.useGravity = true;
            instance.Rigidbody.AddForce(Vector3.up * 3 + Vector3.forward * Random.Range(-1f,2f) + Vector3.right * Random.Range(-1f,2f), ForceMode.Impulse);
            StartCoroutine(instance.ExecuteFalling());
            yield return null;
        }
    }
}
