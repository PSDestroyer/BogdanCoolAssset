using System;
using System.Collections;
using GenesisStudio;
using UnityEngine;

[RequireComponent(typeof(Collider), typeof(Rigidbody))]
public class Collectable : MonoBehaviour
{
    private Rigidbody rb;
    private Collider coll;
    
    public Rigidbody Rigidbody { get { return rb; } }
    public Collider Collider { get { return coll; } }
    public bool isFalling => !Physics.Raycast(transform.position, Vector3.down, out _, 1.5f);
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<Collider>();
        coll.isTrigger = true;
        rb.useGravity = false;
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.Collect();
            Destroy(gameObject);
        }
    }

    public IEnumerator ExecuteFalling()
    {
        while(isFalling)
            yield return null;
        
        rb.useGravity = false;
        rb.isKinematic = true;
    }       
}
