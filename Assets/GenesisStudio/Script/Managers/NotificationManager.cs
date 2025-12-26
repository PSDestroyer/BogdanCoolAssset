using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
namespace GenesisStudio
{
    public class NotificationManager : Singleton<NotificationManager>
    {
        [SerializeField] private TMP_Text message;
        [SerializeField] private Animator _notification;
        [SerializeField] private string animationName;

        private Queue<string> _notifications = new Queue<string>();

        private string _message
        {
            get => message.text;
            set => message.text = value;
        }

        public void ShowNotification(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                // Skip if message is empty
                Debug.LogError($"<color=green>Norification Manager</color>: The notification is empty");
                return;
            } 
            _message = message;
            _notification.Play(animationName);
        }

        private IEnumerator notify(string message)
        {
            _notifications.Enqueue(message);
            while (_notifications.Count > 0)
            {
                _message = _notifications.Dequeue();
                _notification.Play(animationName);

                yield return new WaitForSeconds(GetAnimationLength());
            }
        }
        private float GetAnimationLength()
        {
            // Cache the animation length if it's already retrieved
            Dictionary<string, float> animationLengths = new Dictionary<string, float>();

            if (animationLengths.TryGetValue(animationName, out float cachedLength))
            {
                return cachedLength;
            }

            // Otherwise, calculate it and store it
            float length = 0f;
            AnimationClip[] clips = _notification.runtimeAnimatorController.animationClips;
            foreach (var clip in clips)
            {
                if (clip.name == animationName)
                {
                    length = clip.length;
                    animationLengths[animationName] = length; // Cache the length for later
                    break;
                }
            }

            Debug.Log($"Animation length for {animationName}: {length}");
            return length;
        }

    }
}
