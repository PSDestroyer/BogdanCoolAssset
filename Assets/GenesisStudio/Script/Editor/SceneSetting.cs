using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneSetting 
{
    [MenuItem("Tools/GenesisStudio/Add Gameplay")]
    public static void SetUpScene()
    {
        string path = "Assets/GenesisStudio/Prefabs/Gameplay.prefab";
        var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if(prefab != null)
        {
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            instance.name = "Gameplay";
            instance.transform.position = Vector3.zero;
            EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
        }
        else
        {
            Debug.LogError($"Prefab at {path} not found");
        }
    }
}
