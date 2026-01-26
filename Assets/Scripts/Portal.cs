using GenesisStudio;
using PlatformCharacterController;
using UnityEngine;

public class Portal : LevelLoader
{
    private bool Active;
    
    public void Activate() => Active = true; 
    public void Deactivate() => Active = false;

    public bool entered;
    
    protected override void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out MovementCharacterController player))
        {
            if (entered) return;

            GameManager.Instance.Complete();
            player.Controls(false);
            InputManager.Instance.ChangeMap(Needs.UIMap);

            entered = true;
        }
    }
    
    

    protected override void Load()
    {
        if(Active) 
            base.Load();
    }
}
