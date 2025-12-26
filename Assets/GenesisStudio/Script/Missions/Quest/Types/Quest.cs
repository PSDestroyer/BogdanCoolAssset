using GenesisStudio;
using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Quest : ScriptableObject
{
    protected string _task;
    protected QuestGameObject _questGameObject;
    protected Player _player;
    private UnityEvent _onComplete;
    public string Task => _task;

    public abstract bool IsAlreadyCompleted();

    public void Initialize(QuestParams @params)
    {
        _questGameObject = @params.QuestGameObject;
        _task = @params.Task;
        _player = @params.Player;
        OnInitialize(@params);
    }

    public abstract void OnInitialize(QuestParams @params);
    public abstract void OnComplete();

    public void Complete()
    {
        _questGameObject.Complete();
        _questGameObject = null;
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
}

[Serializable]
public class QuestParams
{
    public QuestGameObject QuestGameObject { get; set;}
    public Player Player { get; set;}
    [field: SerializeField] public string Task { get; private set;}
    [field: SerializeField] public Transform Target_point {get; private set;}
    [field: SerializeField] public NPC Target_npc {get; private set;}
    [field: SerializeField] public ItemData Target_item { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
    [field: SerializeField] public GameObject Target_gameObject { get; private set; }
    [field: SerializeField] public Dialogue Dialogue { get; private set; }

}
