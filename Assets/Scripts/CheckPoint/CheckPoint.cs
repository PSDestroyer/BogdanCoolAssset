using System;
using System.Runtime.InteropServices;
using HalvaStudio.Save;
using PlatformCharacterController;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] Transform spawnPoint;
    
    private CheckPointManager _manager;
    private MovementCharacterController _player;
    
    private CheckPointData CheckPointData =>
        new()
        {
            Health = _player.Health,
            Collected = GameManager.Instance.collected,
            x = spawnPoint.position.x,
            y = spawnPoint.position.y,
            z = spawnPoint.position.z,
            Abilities = _player.AbilityManager.GetActiveAbilities()
        };


    public void Initialize(CheckPointManager manager, MovementCharacterController player)
    {
        _manager = manager;
        _player = player;
    }

    private void Save()
    {
        _manager.SaveData(CheckPointData);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Save();
    }
}