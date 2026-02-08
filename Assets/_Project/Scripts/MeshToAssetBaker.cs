#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;

public static class MeshToAssetBaker
{
    [MenuItem("Tools/Bake Meshes of Selected To Assets")]
    public static void BakeSelected()
    {
        var root = Selection.activeGameObject;
        if (root == null)
        {
            Debug.LogError("Ничего не выбрано в Hierarchy.");
            return;
        }

        const string folder = "Assets/BakedMeshes";
        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets", "BakedMeshes");

        var filters = root.GetComponentsInChildren<MeshFilter>(true);
        int baked = 0;

        foreach (var mf in filters)
        {
            var mesh = mf.sharedMesh;
            if (mesh == null)
                continue;

            // Если это уже ассет — пропускаем
            var assetPathExisting = AssetDatabase.GetAssetPath(mesh);
            if (!string.IsNullOrEmpty(assetPathExisting))
                continue;

            var meshCopy = Object.Instantiate(mesh);
            meshCopy.name = mf.name + "_Baked";

            var path = Path.Combine(folder, meshCopy.name + ".asset").Replace("\\", "/");
            path = AssetDatabase.GenerateUniqueAssetPath(path);

            AssetDatabase.CreateAsset(meshCopy, path);
            mf.sharedMesh = AssetDatabase.LoadAssetAtPath<Mesh>(path);

            baked++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Готово. Запечено мешей: {baked}. Папка: {folder}");
    }
}
#endif
