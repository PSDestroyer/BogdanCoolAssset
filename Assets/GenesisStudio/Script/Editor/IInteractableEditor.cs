using GenesisStudio;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Door))]
public class IInteractableEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        var inter = (Door)target;

        if (GUILayout.Button("Interact"))
        {
            inter.Interact(this);
        }
    }
}
