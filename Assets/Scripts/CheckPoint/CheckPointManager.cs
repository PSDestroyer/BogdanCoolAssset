using System;
using HalvaStudio.Save;
using PlatformCharacterController;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    [SerializeField] CheckPoint[] checkPoints;
    
    SaveManager _save;
    CheckPointData _currentCheckPoint;
    
    
    private void Start()
    {
        _save = SaveManager.Instance;
        _currentCheckPoint = _save.saveData.lastCheckPoint;
        foreach (var checkPoint in checkPoints)
        {
            checkPoint.Initialize(this, (MovementCharacterController)GameManager.Instance.Player);
        }
    }

    public void SaveData(CheckPointData data)
    {
        _currentCheckPoint = data;
        _save.saveData.lastCheckPoint = data;
    }

    public void Load()
    {
        
    }
}