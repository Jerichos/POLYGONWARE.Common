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
            "Graphics/Materials",
            "Graphics/Textures",
            "Models",
            "Prefabs",
            "Scripts",
            "Shaders",
            "UI",
            "Scenes",
            "StreamingAssets"
        };

        if (!AssetDatabase.IsValidFolder(rootPath))
        {
            AssetDatabase.CreateFolder("Assets", subfolderName);
        }

        foreach (var subfolder in subfolders)
        {
            string[] nestedFolders = subfolder.Split('/');
            string currentFolderPath = rootPath;

            foreach (var nestedFolder in nestedFolders)
            {
                currentFolderPath = Path.Combine(currentFolderPath, nestedFolder);

                if (!AssetDatabase.IsValidFolder(currentFolderPath))
                {
                    string parentDirectory = Path.GetDirectoryName(currentFolderPath); 
                    string newFolderName = Path.GetFileName(currentFolderPath);

                    // Use Unity's method to create the folder to ensure meta file creation and asset database updating
                    AssetDatabase.CreateFolder(parentDirectory.Replace("\\", "/"), newFolderName);
                }
            }
        }

        AssetDatabase.Refresh();
    }

}