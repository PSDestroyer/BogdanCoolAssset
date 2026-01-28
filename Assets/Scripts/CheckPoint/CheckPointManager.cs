using System;
using System.Collections.Generic;
using HalvaStudio.Save;
using PlatformCharacterController;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class CheckPointManager
{
    
    [SerializeField] List<CheckPoint> _checkPoints;
    SaveManager _save;
    CheckPointData? _currentCheckPoint;
    MovementCharacterController _controller;

    public void Initialize(MovementCharacterController controller)
    {
        _controller = controller;
        _save = SaveManager.Instance;
        _currentCheckPoint = _save.saveData.lastCheckPoint;
        foreach (var checkPoint in _checkPoints)
        {
            checkPoint.Initialize(this, controller);
        }
    }
    
    public string GetCurrentScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public void SaveData(CheckPointData data)
    {
        _currentCheckPoint = data;
        _save.saveData.lastCheckPoint = data;
    }

    public void Load()
    {
        if (_save.saveData.lastCheckPoint == null) return;
        _currentCheckPoint = _save.saveData.lastCheckPoint;
        _currentCheckPoint.Value.Load(_controller);
    }

    public void Add(CheckPoint checkPoint)
    {
        _checkPoints.Add(checkPoint);
    }
}