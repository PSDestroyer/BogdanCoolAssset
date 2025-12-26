using UnityEngine;
using UnityEditor;
using GenesisStudio;

public class CustomGameObjectMenu
{
    // Adds a new item to the "GameObject" menu
    [MenuItem("GameObject/Trigger", false, 10)]
    static void CreateQuestTrigger(MenuCommand menuCommand)
    {
        // Create a new empty GameObject
        GameObject go = new GameObject("Trigger");

        // Add your custom component
        go.AddComponent<BoxCollider>().isTrigger = true;
        go.AddComponent<Trigger>();

        // Register the creation for undo
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        Selection.activeObject = go;
    }
}
