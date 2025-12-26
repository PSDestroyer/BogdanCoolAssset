#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

[InitializeOnLoad]
public static class GenesisPackageDependencies
{
    static GenesisPackageDependencies()
    {
        AddPackage("com.unity.inputsystem", "1.7.0");
        AddPackage("com.unity.cinemachine", "2.9.7");
    }

    private static void AddPackage(string packageName, string version)
    {
        string manifestPath = Path.Combine(Application.dataPath, "../Packages/manifest.json");
        string manifest = File.ReadAllText(manifestPath);

        if (!manifest.Contains(packageName))
        {
            Debug.Log($"📦 Adding missing dependency: {packageName}@{version}");
            string toInsert = $"\"{packageName}\": \"{version}\",";
            int index = manifest.IndexOf("\"dependencies\": {") + "\"dependencies\": {".Length;
            manifest = manifest.Insert(index, "\n    " + toInsert);
            File.WriteAllText(manifestPath, manifest);
            AssetDatabase.Refresh();
        }
    }
}
#endif