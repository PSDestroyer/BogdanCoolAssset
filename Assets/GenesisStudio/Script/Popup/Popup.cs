using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GenesisStudio
{
    public class Popup : Singleton<Popup>
    {
        [SerializeField] private GameObject PopupPanel;
        [SerializeField] private TMP_Text PopupQuestion;
        [SerializeField] private Button yes;
        [SerializeField] private Button no;
        
        private TaskCompletionSource<bool> _task;

        private void Start()
        {
            yes.onClick.AddListener(() => { _task.TrySetResult(true);});
            no.onClick.AddListener(() => { _task.TrySetResult(false);});
            PopupPanel.SetActive(false);
        }

        public async Task<bool> ShowPopup(string question)
        {
            PopupPanel.SetActive(true);
            no.Select();
            PopupQuestion.text = question;
            _task = new TaskCompletionSource<bool>();
            
            bool result = await _task.Task;
            PopupPanel.SetActive(false);
            return result;
        }



    }
}
