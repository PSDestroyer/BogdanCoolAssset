using PlatformCharacterController;
using UnityEngine;

public class ShootController : MonoBehaviour
{
    
    // [Header("Trajectory")]
    // public LineRenderer lineRenderer;
    // [SerializeField] int trajectoryResolution = 30;
    // [SerializeField] float timeStep = 0.1f;
    //
    
    [Header("Bomb")]
    public Bomb bombPrefab;
    public float force {get; set; }
    public float height {get; set; }
    
    MovementCharacterController _controller;
    Transform _rHand;
    Transform _cameraTransform;

    public void Shoot()
    {
        var instance = Instantiate(bombPrefab, _rHand.position, Quaternion.identity);
        instance.rb.AddForce(InitialVelocity(), ForceMode.Impulse);
    }

    public void Initialize(MovementCharacterController _controller)
    {
        this._controller = _controller;
        _rHand = _controller.rHand;
        _cameraTransform = _controller.CameraTransform;
        
        // lineRenderer.positionCount = trajectoryResolution;
        // lineRenderer.enabled = true;
    }   
    
    Vector3 InitialVelocity()
    {
        return _controller.transform.forward * force + Vector3.up * height;
    }
    
    // public void Trajectory(bool value)
    // {
    //     if(value) ShowTrajectory();
    //     else lineRenderer.enabled = false;
    // }
    //
    // [ContextMenu("ShowTrajectory")]
    // private void ShowTrajectory()
    // {
    //     Vector3 startPos = _rHand.position;
    //     Vector3 velocity = InitialVelocity();
    //     Vector3 gravity = Physics.gravity;
    //
    //     Vector3 prevPoint = startPos;
    //
    //     for (int i = 0; i < trajectoryResolution; i++)
    //     {
    //         float t = i * timeStep;
    //
    //         Vector3 point =
    //             startPos +
    //             velocity * t +
    //             0.5f * gravity * t * t;
    //
    //         lineRenderer.SetPosition(i, point);
    //
    //         // Optional: collision prediction
    //         if (Physics.Raycast(prevPoint, point - prevPoint, out RaycastHit hit))
    //         {
    //             lineRenderer.SetPosition(i, hit.point);
    //             lineRenderer.positionCount = i + 1;
    //             break;
    //         }
    //
    //         prevPoint = point;
    //     }
    //
    //     lineRenderer.enabled = true;
    // }
}