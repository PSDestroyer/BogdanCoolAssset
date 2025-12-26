using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueDataSO", menuName = "Genesis Studio/Dialogue/DialogueDataSO")]
public class Dialogue : ScriptableObject
{
    [field: SerializeField] private DialogueData[] phrases;

    public IReadOnlyList<DialogueData> Phrases => phrases.ToList();

    [Serializable]
    public class DialogueData
    {
        [SerializeField] Actor actor;
        [SerializeField] string phrase;
        [SerializeField] AudioClip voice;


        public string Phrase
        {
            get
            {
                string hex = actor.ActorColor.ToHexString();
                return $"<color=#{hex}>{actor.ActorName}</color>: {phrase}";
            }
        }

        public IEnumerator Say(Action onEnd = null)
        {
            AudioManager.Instance.Play(voice);
            float lenth = voice.length;
            yield return new WaitForSeconds(lenth);
            onEnd?.Invoke();
        }

    }
}
