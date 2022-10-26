using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AllIn1Lighting)), CanEditMultipleObjects]
public class AllIn1LightingEditor : Editor
{
    SerializedProperty m_NormalStrength, m_NormalSmoothing;
    private GUIStyle guiStyle = new GUIStyle();

    private void OnEnable()
    {
        m_NormalStrength = serializedObject.FindProperty("normalStrenght");
        m_NormalSmoothing = serializedObject.FindProperty("normalSmoothing");
    }

    public override void OnInspectorGUI()
    {
        Texture2D imageInspector = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/AllIn1SpriteLighting/Textures/CustomEditorImageLighting.png", typeof(Texture2D));

        guiStyle.fontSize = 15;
        guiStyle.fontStyle = FontStyle.Bold;

        if (imageInspector)
        {
            Rect rect;
            float imageHeight = imageInspector.height;
            float imageWidth = imageInspector.width;
            float aspectRatio = imageHeight / imageWidth;
            rect = GUILayoutUtility.GetRect(imageHeight, aspectRatio * Screen.width * 0.7f);
            EditorGUI.DrawTextureTransparent(rect, imageInspector);
        }

        AllIn1Lighting myScript = (AllIn1Lighting)target;

        if (GUILayout.Button("Deactivate All Effects"))
        {
            for (int i = 0; i < targets.Length; i++)
            {
                (targets[i] as AllIn1Lighting).ClearAllKeywords();
            }
        }


        if (GUILayout.Button("New Clean Material"))
        {
            for (int i = 0; i < targets.Length; i++)
            {
                (targets[i] as AllIn1Lighting).TryCreateNew();
            }
        }


        if (GUILayout.Button("Create New Material With Same Properties (SEE DOC)"))
        {
            for (int i = 0; i < targets.Length; i++)
            {
                (targets[i] as AllIn1Lighting).MakeCopy();
            }
        }

        if (GUILayout.Button("Save Material To Folder (SEE DOC)"))
        {
            for (int i = 0; i < targets.Length; i++)
            {
                (targets[i] as AllIn1Lighting).SaveMaterial();
            }
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Create And Add Normal Map"))
        {
            for (int i = 0; i < targets.Length; i++)
            {
                (targets[i] as AllIn1Lighting).CreateAndAssignNormalMap();
            }
        }
        serializedObject.Update();
        EditorGUILayout.PropertyField(m_NormalStrength, new GUIContent("Normal Strength"), GUILayout.Height(20));
        EditorGUILayout.PropertyField(m_NormalSmoothing, new GUIContent("Normal Blur"), GUILayout.Height(20));
        if (myScript.computingNormal)
        {
            EditorGUILayout.LabelField("Normal Map is currently being created, be patient", guiStyle, GUILayout.Height(40));
        }
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.Space();

        if (GUILayout.Button("REMOVE COMPONENT AND MATERIAL"))
        {
            for (int i = 0; i < targets.Length; i++)
            {
                (targets[i] as AllIn1Lighting).CleanMaterial();
            }
            for (int i = targets.Length - 1; i >= 0; i--)
            {
                DestroyImmediate(targets[i] as AllIn1Lighting);
            }
        }
    }
}
