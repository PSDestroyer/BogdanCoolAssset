using GenesisStudio;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Quest : ScriptableObject
{
    protected string _task;
    protected ICharacter _player;
    protected GameObject _playerGO;
    private UnityEvent _onComplete;
    public string Task => _task;
    public bool IsComplete { get; private set; }
    private string baseTask;

    public abstract bool IsAlreadyCompleted();

    public void Initialize(QuestParams @params)
    {
        IsComplete = false;
        _task = @params.Task;
        baseTask = _task;
        _player = @params.Player;
        if(_player is MonoBehaviour mb) _playerGO = mb.gameObject;
        OnInitialize(@params);
        Debug.Log("Quest Initialized with task" + _task);
        GameEventBus.Instance.OnQuestAdded?.Invoke(this, @params);
    }

    public abstract void OnInitialize(QuestParams @params);
    public abstract void OnComplete();

    public void Complete()
    {
        Debug.Log("Quest Completed with task" + _task);
        IsComplete = true;
        _player = null;
        OnComplete();
    }

    private void OnDestroy()
    {
        Complete();
    }

    public virtual void Update()
    {

    }

    public void AppendToTask(string additional)
    {
        string add = $"{baseTask} {additional}";
        _task = add;
    }
}

[Serializable]
public class QuestParams
{
    public ICharacter Player { get; set;}
    [field: SerializeField] public string Task { get; private set;}
    [field: SerializeField] public Transform Target_point {get; private set;}
    [field: SerializeField] public NPC Target_npc {get; private set;}
    [field: SerializeField] public ItemData Target_item { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
    [field: SerializeField] public GameObject Target_gameObject { get; private set; }
    [field: SerializeField] public Dialogue Dialogue { get; private set; }

}
