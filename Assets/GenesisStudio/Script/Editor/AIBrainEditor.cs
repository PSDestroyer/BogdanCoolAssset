using System;
using GenesisStudio;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;

[CustomEditor(typeof(AIBrain))]
public class AIBrainEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        var brain = (AIBrain)target;

        if (GUILayout.Button("Idle State"))
        {
            brain.stateMachine.ChangeState(brain.stateMachine.idleState);
        }
        
        if (GUILayout.Button("Patrol State"))
        {
            brain.stateMachine.ChangeState(brain.stateMachine.patrolState);
        }
        
        if (GUILayout.Button("Inspect State"))
        {
            brain.stateMachine.ChangeState(brain.stateMachine.interactState);
        }
    }
}
