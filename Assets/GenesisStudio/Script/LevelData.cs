//clasele sunt facute pentru extindere viitoare

using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField] private string LevelName;
    [SerializeField] SceneAsset nextLevel;
    [SerializeField] private bool isFinal;
    public string Scene => nextLevel.name;

    public bool IsFinal => isFinal;


    public string ID()
    {
        return LevelName;
    }
    

}
