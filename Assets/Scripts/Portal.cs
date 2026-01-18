using UnityEngine;

public class Portal : LevelLoader
{
    private bool Active;
    
    public void Activate() => Active = true; 
    public void Deactivate() => Active = false; 
    
    protected override void Load()
    {
        if(Active) 
            base.Load();
    }
}
