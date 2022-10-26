
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Linq;

[CanEditMultipleObjects]
public class SpriteLightingGUI : ShaderGUI
{
    Material targetMat;
    private UnityEngine.Rendering.BlendMode srcMode, dstMode;
    private UnityEngine.Rendering.CompareFunction zTestMode;

    private GUIStyle propertiesStyle, bigLabel = new GUIStyle();
    private const int bigFontSize = 16;
    float tempValue;

    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        targetMat = materialEditor.target as Material;
        string[] oldKeyWords = targetMat.shaderKeywords;
        propertiesStyle = new GUIStyle(EditorStyles.helpBox);
        propertiesStyle.margin = new RectOffset(0, 0, 0, 0);
        bigLabel = new GUIStyle(EditorStyles.boldLabel);
        bigLabel.fontSize = bigFontSize;

        GUILayout.Label("General Properties", bigLabel);
        materialEditor.ShaderProperty(properties[0], properties[0].displayName);
        materialEditor.ShaderProperty(properties[1], properties[1].displayName);
        materialEditor.ShaderProperty(properties[2], properties[2].displayName);
        materialEditor.ShaderProperty(properties[3], properties[3].displayName);
        materialEditor.ShaderProperty(properties[39], properties[39].displayName);

        EditorGUILayout.Separator();
        DrawLine(Color.grey, 1, 3);
        GUILayout.Label("Effects", bigLabel);
        GenericEffect(materialEditor, properties, propertiesStyle, oldKeyWords.Contains("TOON_ON"), "1.Toon Light", "TOON_ON", 8, 10);
        GenericEffect(materialEditor, properties, propertiesStyle, oldKeyWords.Contains("NORMALMAP_ON"), "2.Normal Mapping", "NORMALMAP_ON", 6, 7);
        GenericEffect(materialEditor, properties, propertiesStyle, oldKeyWords.Contains("SPECULAR_ON"), "3.Specular", "SPECULAR_ON", 4, 5);
        GenericEffect(materialEditor, properties, propertiesStyle, oldKeyWords.Contains("GLOW_ON"), "4.Glow", "GLOW_ON", 11, 14);
        GenericEffect(materialEditor, properties, propertiesStyle, oldKeyWords.Contains("OUTLINE_ON"), "5.Outline", "OUTLINE_ON", 15, 20);
        GenericEffect(materialEditor, properties, propertiesStyle, oldKeyWords.Contains("FADE_ON"), "6.Fade", "FADE_ON", 21, 27);
        GenericEffect(materialEditor, properties, propertiesStyle, oldKeyWords.Contains("HSV_ON"), "7.Hue Shift", "HSV_ON", 28, 30);
        GenericEffect(materialEditor, properties, propertiesStyle, oldKeyWords.Contains("HITEFFECT_ON"), "8.Hit Effect", "HITEFFECT_ON", 31, 33);
        GenericEffect(materialEditor, properties, propertiesStyle, oldKeyWords.Contains("WIND_ON"), "9.Grass Movement / Wind", "WIND_ON", 36, 38);

        EditorGUILayout.Separator();
        DrawLine(Color.grey, 1, 3);
        GUILayout.Label("Advanced Configuration", bigLabel);
        if (GUILayout.Button("Back To Default Configuration"))
        {
            MaterialProperty zWrite = ShaderGUI.FindProperty("_ZWrite", properties);
            zWrite.floatValue = 1.0f;
            MaterialProperty zTestM = ShaderGUI.FindProperty("_ZTestMode", properties);
            zTestM.floatValue = (float) UnityEngine.Rendering.CompareFunction.LessEqual;
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        ZWrite(materialEditor, properties);
        ZTest(materialEditor, properties);

        EditorGUILayout.Separator();
        DrawLine(Color.grey, 1, 3);
        materialEditor.RenderQueueField();
    }

    private void ZWrite(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty zWrite = ShaderGUI.FindProperty("_ZWrite", properties);
        bool toggle = zWrite.floatValue > 0.9f ? true : false;
        EditorGUILayout.BeginHorizontal();
        {
            tempValue = zWrite.floatValue;
            toggle = EditorGUILayout.Toggle("Enable Z Write", toggle);
            if (toggle) zWrite.floatValue = 1.0f;
            else zWrite.floatValue = 0.0f;
            if (tempValue != zWrite.floatValue && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        EditorGUILayout.EndHorizontal();
    }

    private void ZTest(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        MaterialProperty zTestM = ShaderGUI.FindProperty("_ZTestMode", properties);
        tempValue = zTestM.floatValue;
        zTestMode = (UnityEngine.Rendering.CompareFunction)zTestM.floatValue;
        zTestMode = (UnityEngine.Rendering.CompareFunction)EditorGUILayout.EnumPopup("Z TestMode", zTestMode);
        zTestM.floatValue = (float)(zTestMode);
        if (tempValue != zTestM.floatValue && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }

    private void GenericEffect(MaterialEditor materialEditor, MaterialProperty[] properties, GUIStyle style, bool toggle, string inspector, string flag, int first, int last)
    {
        bool ini = toggle;
        toggle = EditorGUILayout.BeginToggleGroup(inspector, toggle);
        if (ini != toggle && !Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        if (toggle)
        {
            targetMat.EnableKeyword(flag);
            if (first > 0)
            {
                EditorGUILayout.BeginVertical(style);
                {
                    for (int i = first; i <= last; i++) materialEditor.ShaderProperty(properties[i], properties[i].displayName);
                }
                EditorGUILayout.EndVertical();
            }
        }
        else targetMat.DisableKeyword(flag);
        EditorGUILayout.EndToggleGroup();
    }

    private void DrawLine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += (padding / 2);
        r.x -= 2;
        r.width += 6;
        EditorGUI.DrawRect(r, color);
    }
}