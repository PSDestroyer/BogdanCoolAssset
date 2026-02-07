using System.Collections.Generic;
using PlatformCharacterController;
using UnityEngine;

public struct CheckPointData
{
    public float Health;
    public int Collected;
    public List<Ability> Abilities;
    public float x,y,z;

    private Vector3 GetPosition() => new Vector3(x, y, z);
    
    public void Load(MovementCharacterController player)
    {
        // player.Wrap(GetPosition());
        player.Health = Health;
        player.Collected = Collected;
        player.AbilityManager.Clear();
        foreach (Ability ability in Abilities)
        {
            player.AbilityManager.AddAbility(ability);
        }
    }
}
