using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestGameObject : MonoBehaviour
{
    [SerializeField] TMP_Text TMP_Task;
    [SerializeField] Toggle tgl_isComplete;

    private Quest _questData;
    public Quest Data => _questData;
    public QuestParams DataParams;
    public bool IsComplete { get => tgl_isComplete.isOn; set => tgl_isComplete.isOn = value; }
    private string _task 
    {
        get => TMP_Task.text;
        set
        {
            
            TMP_Task.text = value;
        }
    }
    string baseTask;

    int CollectAmount = 0;

    public void Complete()
    {
        IsComplete = true;
    }

    public void Initialize(Quest data, QuestParams @params)
    {
        IsComplete = false;
        _questData = data;
        DataParams = @params;   
        _task = @params.Task;
        baseTask = @params.Task;
        if(data is CollectableQuest cq)
        {
            CollectAmount = cq.CollectAmount;
            string newTask = $"{baseTask} 0/{CollectAmount}";
            _task = newTask;
        }
    }

    public void AddAditionalInformationToTask(string additional)
    {
        string add = $"{baseTask} {additional}";
        _task = add;
    }

}
