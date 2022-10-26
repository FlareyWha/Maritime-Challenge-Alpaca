#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class AllIn1LightWindow : EditorWindow
{
    [MenuItem("Window/AllIn1LightWindow")]
    public static void ShowAllIn1LightWindowWindow()
    {
        GetWindow<AllIn1LightWindow>("All In 1 Lighting Window");
    }

    private DefaultAsset materialTargetFolder = null;

    private void OnGUI()
    {
        GUILayout.Label("Through this window you'll be able to set the folders where the asset will save it's Materials and Normal Maps",
            EditorStyles.boldLabel);

        GUILayout.Space(20);
        GUILayout.Label("Select the folder where new Materials will be saved when the Save Material to Folder button of the asset component is pressed", EditorStyles.boldLabel);
        HandleSaveFolderEditorPref("All1LightMaterials", "Assets/AllIn1SpriteLighting/Materials", "Material");

        GUILayout.Space(20);
        GUILayout.Label("Select the folder where new Normal Maps will be saved when the Create Normal Map button of the asset component is pressed", EditorStyles.boldLabel);
        HandleSaveFolderEditorPref("All1LightNormals", "Assets/AllIn1SpriteLighting/NormalMaps", "Normal Maps");
    }

    private void HandleSaveFolderEditorPref(string keyName, string defaultPath, string logsFeatureName)
    {
        if (!PlayerPrefs.HasKey(keyName)) PlayerPrefs.SetString(keyName, defaultPath);
        materialTargetFolder = (DefaultAsset)AssetDatabase.LoadAssetAtPath(PlayerPrefs.GetString(keyName), typeof(DefaultAsset));
        if (materialTargetFolder == null)
        {
            PlayerPrefs.SetString(keyName, defaultPath);
            materialTargetFolder = (DefaultAsset)AssetDatabase.LoadAssetAtPath(PlayerPrefs.GetString(keyName), typeof(DefaultAsset));
            if (materialTargetFolder == null)
            {
                materialTargetFolder = (DefaultAsset)AssetDatabase.LoadAssetAtPath("Assets/", typeof(DefaultAsset));
                if (materialTargetFolder == null) Debug.LogError("The desired save folder doesn't exist. Go to Window -> AllIn1ShaderWindow and set a valid folder");
                else PlayerPrefs.SetString("Assets/", defaultPath);
            }
        }
        materialTargetFolder = (DefaultAsset)EditorGUILayout.ObjectField("New " + logsFeatureName + " Folder", materialTargetFolder, typeof(DefaultAsset), false);

        if (materialTargetFolder != null && IsAssetAFolder(materialTargetFolder))
        {
            string path = AssetDatabase.GetAssetPath(materialTargetFolder);
            PlayerPrefs.SetString(keyName, path);
            EditorGUILayout.HelpBox("Valid folder! " + logsFeatureName + " save path: " + path, MessageType.Info, true);
        }
        else EditorGUILayout.HelpBox("Select the new " + logsFeatureName + " Folder", MessageType.Warning, true);
    }

    private static bool IsAssetAFolder(Object obj)
    {
        string path = "";

        if (obj == null) return false;

        path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

        if (path.Length > 0)
        {
            if (Directory.Exists(path)) return true;
            else return false;
        }
        return false;
    }
}
#endif