using UnityEditor;
using UnityEngine;
using GenesisStudio;
using System.Collections.Generic;

[CustomEditor(typeof(QueuedEvent))]
public class QueuedEventEditor : Editor
{
    private SerializedProperty queuedEventsProp;
    private SerializedProperty beginOnStart;
    private bool showEvents = true;

    private void OnEnable()
    {
        queuedEventsProp = serializedObject.FindProperty("QueuedEvents");
        beginOnStart = serializedObject.FindProperty("beginOnStart");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.LabelField("Queued Event Sequence", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(beginOnStart);
        showEvents = EditorGUILayout.Foldout(showEvents, "Queued Events");
        if (showEvents)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < queuedEventsProp.arraySize; i++)
            {
                SerializedProperty eventProp = queuedEventsProp.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"Event {i + 1}", EditorStyles.boldLabel);

                // Move Up button
                GUI.enabled = i > 0;
                if (GUILayout.Button("▲", GUILayout.Width(30)))
                {
                    queuedEventsProp.MoveArrayElement(i, i - 1);
                }
                GUI.enabled = true;

                // Move Down button
                GUI.enabled = i < queuedEventsProp.arraySize - 1;
                if (GUILayout.Button("▼", GUILayout.Width(30)))
                {
                    queuedEventsProp.MoveArrayElement(i, i + 1);
                }
                GUI.enabled = true;

                if (GUILayout.Button("Remove", GUILayout.Width(60)))
                {
                    queuedEventsProp.DeleteArrayElementAtIndex(i);
                    break;
                }
                EditorGUILayout.EndHorizontal();

                DrawQueuedEventData(eventProp);
                EditorGUILayout.EndVertical();
            }
            EditorGUI.indentLevel--;

            if (GUILayout.Button("Add Event"))
            {
                queuedEventsProp.InsertArrayElementAtIndex(queuedEventsProp.arraySize);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawQueuedEventData(SerializedProperty eventProp)
    {
        SerializedProperty eventTypeProp = eventProp.FindPropertyRelative("eventType");
        SerializedProperty delayProp = eventProp.FindPropertyRelative("delay");
        SerializedProperty animationNameProp = eventProp.FindPropertyRelative("AnimationName");
        SerializedProperty targetAnimator = eventProp.FindPropertyRelative("target_animator");
        SerializedProperty soundNameProp = eventProp.FindPropertyRelative("SoundName");
        SerializedProperty targetPointProp = eventProp.FindPropertyRelative("TargetPoint");
        SerializedProperty targetNPCProp = eventProp.FindPropertyRelative("target_NPC");
        SerializedProperty onExecuteProp = eventProp.FindPropertyRelative("onExecute");

        EditorGUILayout.PropertyField(eventTypeProp);
        EditorGUILayout.PropertyField(delayProp);

        EditorGUILayout.Space(4);

        var eventType = (QueuedEvent.QueueEventType)eventTypeProp.enumValueIndex;
        switch (eventType)
        {
            case QueuedEvent.QueueEventType.Animation:
                EditorGUILayout.PropertyField(animationNameProp);
                EditorGUILayout.PropertyField(targetAnimator);
                break;
            case QueuedEvent.QueueEventType.PlaySound:
                EditorGUILayout.PropertyField(soundNameProp);
                break;
            case QueuedEvent.QueueEventType.NPCGoToPoint:
                EditorGUILayout.PropertyField(targetPointProp);
                EditorGUILayout.PropertyField(targetNPCProp);
                break;
            case QueuedEvent.QueueEventType.Action:
                EditorGUILayout.PropertyField(onExecuteProp);
                break;
        }
    }
}
