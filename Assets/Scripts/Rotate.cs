using UnityEngine;

public class Rotate : MonoBehaviour
{
    [SerializeField] private float speed = 180; 
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
