using UnityEditor;
using UnityEngine;
using System.IO;

public class CreateFolderStructure : EditorWindow
{
    private string subfolderName = "POLYGONWARE.Spectra";

    [MenuItem("POLYGONWARE/Create Spectra Folder Structure")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateFolderStructure), true, "Enter Subfolder Name");
    }

    void OnGUI()
    {
        subfolderName = EditorGUILayout.TextField("Subfolder Name:", subfolderName);

        if (GUILayout.Button("Create"))
        {
            CreateFolders(subfolderName);
            Close();
        }
    }

    void CreateFolders(string subfolderName)
    {
        string rootPath = "Assets/" + subfolderName;
        string[] subfolders = {
            "Animations",
            "Audio",
            "Materials",
            "Models",
            "Prefabs",
            "Scripts",
            "Shaders",
            "Textures",
            "UI",
            "Scenes",
            "StreamingAssets"
        };

        if (!AssetDatabase.IsValidFolder(rootPath))
        {
            Directory.CreateDirectory(rootPath);
        }

        foreach (var folder in subfolders)
        {
            string folderPath = Path.Combine(rootPath, folder);
            
            if (!AssetDatabase.IsValidFolder(folderPath))
            {
                AssetDatabase.CreateFolder(rootPath, folder);
            }
        }

        AssetDatabase.Refresh();
    }
}