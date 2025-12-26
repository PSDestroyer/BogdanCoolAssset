using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.Button;

public class DialogueManager : Singleton<DialogueManager>
{
    [Header("UI")]
    [SerializeField] TMP_Text TMP_Phrase;

    private Dialogue _activeDialogue;
    private bool next;
    private Coroutine _activeDialogueCoroutine;
    
    string Phrase
    {
        set
        {
            if(value == null)
            {
                TMP_Phrase.text = "";
                TMP_Phrase.gameObject.SetActive(false);
                return;
            }
            TMP_Phrase.text = value;
        }
    }


    public void StartDialogue(Dialogue dialogue)
    {
        next = false;
        _activeDialogue = dialogue;
        _activeDialogueCoroutine ??= StartCoroutine(DialogueCoroutine(_activeDialogue));
    }


    private IEnumerator DialogueCoroutine(Dialogue dialogue, bool waitForNext = false)
    {
        Queue<Dialogue.DialogueData> phrases = new Queue<Dialogue.DialogueData>(dialogue.Phrases);
        while (phrases.Count > 0)
        {
            var dData = phrases.Dequeue();
            Phrase = dData.Phrase;
            yield return dData.Say(waitForNext ? Continue : null);
            
            if (waitForNext)
                yield return new WaitUntil(() => next);
        }
        Phrase = null;
        _activeDialogue = null;

    }

    public void Continue()
    {
        next = true;
    }
}


