using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GenesisStudio
{
    public class QueuedEvent : MonoBehaviour
    {
        public bool beginOnStart = true;
        private Dictionary<QueueEventType, Func<QueuedEventData, IEnumerator>> Events;
        [SerializeField] private List<QueuedEventData> QueuedEvents = new List<QueuedEventData>();
        private Queue<QueuedEventData> _queuedEvents = new Queue<QueuedEventData>();
        private Coroutine _executionCoroutine;

        private void Awake()
        {
            Events = new Dictionary<QueueEventType, Func<QueuedEventData, IEnumerator>>
            {
                { QueueEventType.PlaySound, (parametres) => PlayAudio(parametres)},
                { QueueEventType.Action, (parametres) => { parametres.onExecute?.Invoke(); return null; } },
                { QueueEventType.Animation, (parametres) => PlayAnimation(parametres) },
                { QueueEventType.NPCGoToPoint, (parametres) => NPC_GoToPoint(parametres) },
            };
        }

        private IEnumerator PlayAnimation(QueuedEventData parametres)
        {
            string animationName = parametres.AnimationName;
            Animator animator = parametres.target_animator;

            if (animator == null || string.IsNullOrEmpty(animationName))
            {
                Debug.LogWarning("Animator or AnimationName is null in PlayAnimation event.");
                yield break;
            }

            animator.Play(animationName);
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        }

        private IEnumerator NPC_GoToPoint(QueuedEventData parametres)
        {
            NPC npc = parametres.target_NPC;
            Transform point = parametres.TargetPoint;

            if (npc == null || point == null)
            {
                Debug.LogWarning("NPC or TargetPoint is null in NPCGoToPoint event.");
                yield break;
            }

            npc.Move(point.position);
            yield return new WaitUntil(() => npc.ReachedDestination);
        }

        private IEnumerator PlayAudio(QueuedEventData parametres)
        {
            float soundLenght = AudioManager.Instance.PlaySound(parametres.SoundName).clip.length;
            yield return new WaitForSeconds(soundLenght);
        }

        private void Start()
        {
            _queuedEvents = new Queue<QueuedEventData>(QueuedEvents);
            if (beginOnStart)
                Begin();
        }

        public void Begin()
        {
            if (_executionCoroutine == null)
                _executionCoroutine = StartCoroutine(ExecuteEvents());
        }

        private IEnumerator ExecuteEvents()
        {
            while (_queuedEvents.Count > 0)
            {
                var queuedEvent = _queuedEvents.Dequeue();
                yield return new WaitForSeconds(queuedEvent.delay);
                if (Events.TryGetValue(queuedEvent.eventType, out var action))
                {
                    yield return action.Invoke(queuedEvent);
                }
                else
                {
                    Debug.LogWarning($"No action defined for event type: {queuedEvent.eventType}");
                }
            }
        }

        [Serializable]
        private struct QueuedEventData
        {
            public QueueEventType eventType;
            public float delay;

            // Additional parameters can be added here as needed

            public Animator target_animator; // Used if eventType is Animation
            public string AnimationName; // Used if eventType is Animation  

            public string SoundName; // Used if eventType is PlaySound

            public Transform TargetPoint; // Used if eventType is NPCGoToPoint
            public NPC target_NPC; // Used if eventType is NPCGoToPoint

            public UnityEvent onExecute; // Used for custom actions or other event types
        }

        public enum QueueEventType
        {
            PlaySound,
            Action,
            Animation,
            NPCGoToPoint,
        }

    }
}
