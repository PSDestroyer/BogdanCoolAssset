using System.Collections;
using GenesisStudio;
using UnityEngine;

public class FindCollectableAbility : Ability
{
    [SerializeField] private GameObject xRayCamera;
    [SerializeField] private LayerMask collectableLayerMask;
    [SerializeField,  Range(2, 8f)] private float range, showTime;
    
    private Collider[] hits;
    
    
    protected override void Initialize()
    {
        InputManager.Instance.Subscribe(Needs.Interact, Use);    
        
        xRayCamera.SetActive(false);
    }

    protected override IEnumerator C_Use()
    {
        hits = Physics.OverlapSphere(_controller.transform.position, range, collectableLayerMask);
        xRayCamera.SetActive(true);
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out CollectableHighlight ch))
            {
                ch.EnableHighlight();
                yield return null;
            }
        }

        yield return new WaitForSeconds(showTime);
        
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out CollectableHighlight ch))
            {
                ch.DisableHighlight();
                yield return null;
            }
        }
        xRayCamera.SetActive(false);
    }
}
