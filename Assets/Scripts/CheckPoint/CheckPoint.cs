using System;
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
            health = _player.Health,
            bananas = GameManager.Instance.collected,
            spawnPoint = spawnPoint,
            abilities = _player.AbilityManager.GetActiveAbilities()
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