using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

[ExecuteInEditMode]
[AddComponentMenu("AllIn1SpriteLighting/AddAllIn1Lighting")]
public class AllIn1Lighting : MonoBehaviour
{
    private Material currMaterial, prevMaterial;
    private bool matAssigned = false, destroyed = false;
    private enum AfterSetAction { Clear, CopyMaterial, Reset };
    [Range(1f, 20f)] public float normalStrenght = 5f;
    [Range(0f, 3f)] public int normalSmoothing = 1;
    public bool computingNormal = false;

#if UNITY_EDITOR
    private static float timeLastReload = -1f;
    private void Start()
    {
        if (timeLastReload < 0) timeLastReload = Time.time;
    }

    private void Update()
    {
        if (matAssigned || Application.isPlaying || !gameObject.activeSelf) return;
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            if (r.sharedMaterial == null) return;
            if (!r.sharedMaterial.shader.name.Contains("Lighting"))
            {
                MakeNewMaterial();
            }
            else matAssigned = true;
            EnableShadows(r);
        }
        else
        {
            Image img = GetComponent<Image>();
            if (img != null)
            {
                if (img.material.name.Contains("Default")) MakeNewMaterial();
                else matAssigned = true;
            }
        }
    }

#endif

    public void MakeNewMaterial()
    {
        SetMaterial(AfterSetAction.Clear);
    }

    public void MakeCopy()
    {
        SetMaterial(AfterSetAction.CopyMaterial);
    }

    private void ResetAllProperties()
    {
        SetMaterial(AfterSetAction.Reset);
    }

    private void SetMaterial(AfterSetAction action)
    {
        Shader allIn1Shader = Resources.Load("AllIn1SpriteLighting", typeof(Shader)) as Shader;
        if (!Application.isPlaying && Application.isEditor && allIn1Shader != null)
        {
            bool rendererExists = false;
            Renderer r = GetComponent<Renderer>();
            if (r != null)
            {
                rendererExists = true;
                prevMaterial = new Material(r.sharedMaterial);
                currMaterial = new Material(allIn1Shader);
                GetComponent<Renderer>().sharedMaterial = currMaterial;
                GetComponent<Renderer>().sharedMaterial.hideFlags = HideFlags.None;
                matAssigned = true;
                DoAfterSetAction(action);
                EnableShadows(r);
            }
            else
            {
                Image img = GetComponent<Image>();
                if (img != null)
                {
                    rendererExists = true;
                    prevMaterial = new Material(img.material);
                    currMaterial = new Material(allIn1Shader);
                    img.material = currMaterial;
                    img.material.hideFlags = HideFlags.None;
                    matAssigned = true;
                    DoAfterSetAction(action);
                }
            }
            if (!rendererExists)
            {
                MissingRenderer();
                return;
            }
            else
            {
                SetSceneDirty();
            }
        }
        else if (allIn1Shader == null)
        {
            Debug.LogError("Make sure the AllIn1SpriteShader file is inside the Resource folder!");
        }
    }

    private void DoAfterSetAction(AfterSetAction action)
    {
        switch (action)
        {
            case AfterSetAction.Clear:
                ClearAllKeywords();
                break;
            case AfterSetAction.CopyMaterial:
                currMaterial.CopyPropertiesFromMaterial(prevMaterial);
                break;
        }
    }

    public void TryCreateNew()
    {
        bool rendererExists = false;
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            rendererExists = true;
            r = GetComponent<Renderer>();
            if (r != null && r.sharedMaterial != null && r.sharedMaterial.name.Contains("AllIn1"))
            {
                ResetAllProperties();
                ClearAllKeywords();
                EnableShadows(r);
            }
            else
            {
                CleanMaterial();
                MakeNewMaterial();
            }
        }
        else
        {
            Image img = GetComponent<Image>();
            if (img != null)
            {
                rendererExists = true;
                if (img.material.name.Contains("AllIn1"))
                {
                    ResetAllProperties();
                    ClearAllKeywords();
                }
                else MakeNewMaterial();
            }
        }
        if (!rendererExists)
        {
            MissingRenderer();
        }
    }

    private static void EnableShadows(Renderer r)
    {
        r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        r.receiveShadows = true;
    }

    public void ClearAllKeywords()
    {
        SetKeyword("TOON_ON");
        SetKeyword("NORMALMAP_ON");
        SetKeyword("SPECULAR_ON");
        SetKeyword("GLOW_ON");
        SetKeyword("OUTLINE_ON");
        SetKeyword("FADE_ON");
        SetKeyword("HSV_ON");
        SetKeyword("HITEFFECT_ON");
        SetKeyword("WIND_ON");
        SetSceneDirty();
    }

    private void SetKeyword(string keyword, bool state = false)
    {
        if (destroyed) return;
        if (currMaterial == null)
        {
            FindCurrMaterial();
            if (currMaterial == null)
            {
                MissingRenderer();
                return;
            }
        }
        if (!state) currMaterial.DisableKeyword(keyword);
        else currMaterial.EnableKeyword(keyword);
    }

    private void FindCurrMaterial()
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            currMaterial = r.sharedMaterial;
            matAssigned = true;
        }
        else
        {
            Image img = GetComponent<Image>();
            if (img != null)
            {
                currMaterial = img.material;
                matAssigned = true;
            }
        }
    }

    public void CleanMaterial()
    {
        Renderer r = GetComponent<Renderer>();
        if (r != null)
        {
            r.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
            matAssigned = false;
        }
        else
        {
            Image img = GetComponent<Image>();
            if (img != null)
            {
                img.material = new Material(Shader.Find("Sprites/Default"));
                matAssigned = false;
            }
        }
        SetSceneDirty();
    }

    public void SaveMaterial()
    {
#if UNITY_EDITOR
        string path = "Assets/AllIn1SpriteLighting/Materials/";
        if (PlayerPrefs.HasKey("All1LightMaterials")) path = PlayerPrefs.GetString("All1LightMaterials") + "/";
        if (!System.IO.Directory.Exists(path))
        {
            EditorUtility.DisplayDialog("The desired folder doesn't exist",
                   "Go to Window -> AllIn1LightWindow and set a valid folder", "Ok");
            return;
        }
        path += gameObject.name;
        string fullPath = path + ".mat";
        if (System.IO.File.Exists(fullPath))
        {
            SaveMaterialWithOtherName(path);
        }
        else DoSaving(fullPath);
#endif
    }

    private void SaveMaterialWithOtherName(string path, int i = 1)
    {
        int number = i;
        string newPath = path + number.ToString();
        string fullPath = newPath + ".mat";
        if (System.IO.File.Exists(fullPath))
        {
            number++;
            SaveMaterialWithOtherName(path, number);
        }
        else
        {
            DoSaving(fullPath);
        }
    }

    private void DoSaving(string fileName)
    {
#if UNITY_EDITOR
        bool rendererExists = false;
        Renderer r = GetComponent<Renderer>();
        Material matToSave = null;
        if (r != null)
        {
            rendererExists = true;
            matToSave = r.sharedMaterial;
        }
        else
        {
            Image img = GetComponent<Image>();
            if (img != null)
            {
                rendererExists = true;
                matToSave = img.material;
            }
        }
        if (!rendererExists)
        {
            MissingRenderer();
        }
        else
        {
            if (!AssetDatabase.Contains(matToSave))
            {
                AssetDatabase.CreateAsset(matToSave, fileName);
                Debug.Log(fileName + " has been saved!");
            }
            else
            {
                EditorUtility.DisplayDialog("Material instance already exists",
                   "The following material already exists\n No action will be performed", "Ok");
            }
        }
#endif
    }

    private void SetSceneDirty()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
#endif
    }

    private void MissingRenderer()
    {
#if UNITY_EDITOR
        EditorUtility.DisplayDialog("Missing Renderer", "This GameObject (" +
                            gameObject.name + ") has no Renderer or UI Image component. This AllIn1Lighting component will be removed.", "Ok");
        destroyed = true;
        DestroyImmediate(this);
        return;
#endif
    }

    #region normalMapCreator
    protected virtual void OnEnable()
    {
#if UNITY_EDITOR
        EditorApplication.update += OnEditorUpdate;
#endif
    }

    protected virtual void OnDisable()
    {
#if UNITY_EDITOR
        EditorApplication.update -= OnEditorUpdate;
#endif
    }

    bool needToWait;
    int waitingCycles;
    int timesWeWaited;
    protected virtual void OnEditorUpdate()
    {
        if (computingNormal)
        {
            if (needToWait)
            {
                waitingCycles++;
                if (waitingCycles > 5)
                {
                    needToWait = false;
                    timesWeWaited++;
                }
            }
            else
            {
                if (timesWeWaited == 1) SetNewNormalTexture2();
                if (timesWeWaited == 2) SetNewNormalTexture3();
                if (timesWeWaited == 3) SetNewNormalTexture4();
                needToWait = true;
            }
        }
    }

    SpriteRenderer normalMapSr;
    Renderer normalMapRenderer;
    bool isSpriteRenderer;
    public void CreateAndAssignNormalMap()
    {
#if UNITY_EDITOR
        normalMapSr = GetComponent<SpriteRenderer>();
        normalMapRenderer = GetComponent<Renderer>();
        if (normalMapSr != null)
        {
            isSpriteRenderer = true;
            SetNewNormalTexture();
        }
        else if (normalMapRenderer != null)
        {
            isSpriteRenderer = false;
            SetNewNormalTexture();
        }
        else
        {
            EditorUtility.DisplayDialog("Missing Renderer", "This GameObject (" +
                                gameObject.name + ") has no Renderer. This AllIn1Lighting component will be removed.", "Ok");
            destroyed = true;
            DestroyImmediate(this);
        }
#endif
    }

    string path;
    private void SetNewNormalTexture()
    {
#if UNITY_EDITOR
        path = "Assets/AllIn1SpriteLighting/NormalMaps/";
        if (PlayerPrefs.HasKey("All1LightNormals")) path = PlayerPrefs.GetString("All1LightNormals") + "/";
        if (!System.IO.Directory.Exists(path))
        {
            EditorUtility.DisplayDialog("The desired folder doesn't exist",
                   "Go to Window -> AllIn1LightWindow and set a valid folder", "Ok");
            return;
        }
#else
        computingNormal = false;
        return;
#endif

        computingNormal = true;
        needToWait = true;
        waitingCycles = 0;
        timesWeWaited = 0;
    }

#if UNITY_EDITOR
    TextureImporter importer;
    Texture2D mainTex2D;
#endif
    private void SetNewNormalTexture2()
    {
#if UNITY_EDITOR
        if (!isSpriteRenderer)
        {
            mainTex2D = (Texture2D)normalMapRenderer.sharedMaterial.GetTexture("_MainTex");
            importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(mainTex2D)) as TextureImporter;
        }
        else importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(normalMapSr.sprite)) as TextureImporter;

        importer.isReadable = true;
        importer.SaveAndReimport();
#endif
    }

    string subPath;
    private void SetNewNormalTexture3()
    {
#if UNITY_EDITOR
        Texture2D normalM = null;
        if (isSpriteRenderer) normalM = CreateNormalmap(normalMapSr.sprite.texture, normalStrenght, normalSmoothing);
        else normalM = CreateNormalmap(mainTex2D, normalStrenght, normalSmoothing);

        byte[] bytes = normalM.EncodeToPNG();

        path += gameObject.name;
        subPath = path + ".png";
        string dataPath = Application.dataPath;
        dataPath = dataPath.Replace("/Assets", "/");
        string fullPath = dataPath + subPath;

        File.WriteAllBytes(fullPath, bytes);
        AssetDatabase.ImportAsset(subPath);
        AssetDatabase.Refresh();
#endif
    }

    private void SetNewNormalTexture4()
    {
#if UNITY_EDITOR
        importer = AssetImporter.GetAtPath(subPath) as TextureImporter;
        importer.textureType = TextureImporterType.NormalMap;
        importer.wrapMode = TextureWrapMode.Repeat;
        importer.SaveAndReimport();

        if (currMaterial == null)
        {
            FindCurrMaterial();
            if (currMaterial == null)
            {
                MissingRenderer();
                return;
            }
        }
        currMaterial.EnableKeyword("NORMALMAP_ON");
        Texture2D normalTex = (Texture2D)AssetDatabase.LoadAssetAtPath(subPath, typeof(Texture2D));
        currMaterial.SetTexture("_NormalsTex", normalTex);

        Debug.Log("Normal texture saved to: " + subPath);

        computingNormal = false;
#endif
    }

    private Texture2D CreateNormalmap(Texture2D t, float normalMult = 5f, int normalSmooth = 0)
    {
        Color[] pixels = new Color[t.width * t.height];
        Texture2D texNormal = new Texture2D(t.width, t.height, TextureFormat.RGB24, false, false);
        Vector3 vScale = new Vector3(0.3333f, 0.3333f, 0.3333f);

        for (int y = 0; y < t.height; y++)
        {
            for (int x = 0; x < t.width; x++)
            {
                Color tc = t.GetPixel(x - 1, y - 1);
                Vector3 cSampleNegXNegY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x, y - 1);
                Vector3 cSampleZerXNegY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x + 1, y - 1);
                Vector3 cSamplePosXNegY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x - 1, y);
                Vector3 cSampleNegXZerY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x + 1, y);
                Vector3 cSamplePosXZerY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x - 1, y + 1);
                Vector3 cSampleNegXPosY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x, y + 1);
                Vector3 cSampleZerXPosY = new Vector3(tc.r, tc.g, tc.g);
                tc = t.GetPixel(x + 1, y + 1);
                Vector3 cSamplePosXPosY = new Vector3(tc.r, tc.g, tc.g);
                float fSampleNegXNegY = Vector3.Dot(cSampleNegXNegY, vScale);
                float fSampleZerXNegY = Vector3.Dot(cSampleZerXNegY, vScale);
                float fSamplePosXNegY = Vector3.Dot(cSamplePosXNegY, vScale);
                float fSampleNegXZerY = Vector3.Dot(cSampleNegXZerY, vScale);
                float fSamplePosXZerY = Vector3.Dot(cSamplePosXZerY, vScale);
                float fSampleNegXPosY = Vector3.Dot(cSampleNegXPosY, vScale);
                float fSampleZerXPosY = Vector3.Dot(cSampleZerXPosY, vScale);
                float fSamplePosXPosY = Vector3.Dot(cSamplePosXPosY, vScale);
                float edgeX = (fSampleNegXNegY - fSamplePosXNegY) * 0.25f + (fSampleNegXZerY - fSamplePosXZerY) * 0.5f + (fSampleNegXPosY - fSamplePosXPosY) * 0.25f;
                float edgeY = (fSampleNegXNegY - fSampleNegXPosY) * 0.25f + (fSampleZerXNegY - fSampleZerXPosY) * 0.5f + (fSamplePosXNegY - fSamplePosXPosY) * 0.25f;
                Vector2 vEdge = new Vector2(edgeX, edgeY) * normalMult;
                Vector3 norm = new Vector3(vEdge.x, vEdge.y, 1.0f).normalized;
                Color c = new Color(norm.x * 0.5f + 0.5f, norm.y * 0.5f + 0.5f, norm.z * 0.5f + 0.5f, 1);
                pixels[x + y * t.width] = c;
            }
        }

        if (normalSmooth > 0f)
        {
            float step = 0.00390625f * normalSmooth;
            for (int y = 0; y < t.height; y++)
            {
                for (int x = 0; x < t.width; x++)
                {
                    float pixelsToAverage = 0.0f;
                    Color c = pixels[(x + 0) + ((y + 0) * t.width)];
                    pixelsToAverage++;
                    if (x - normalSmooth > 0)
                    {
                        if (y - normalSmooth > 0)
                        {
                            c += pixels[(x - normalSmooth) + ((y - normalSmooth) * t.width)];
                            pixelsToAverage++;
                        }
                        c += pixels[(x - normalSmooth) + ((y + 0) * t.width)];
                        pixelsToAverage++;
                        if (y + normalSmooth < t.height)
                        {
                            c += pixels[(x - normalSmooth) + ((y + normalSmooth) * t.width)];
                            pixelsToAverage++;
                        }
                    }
                    if (y - normalSmooth > 0)
                    {
                        c += pixels[(x + 0) + ((y - normalSmooth) * t.width)];
                        pixelsToAverage++;
                    }
                    if (y + normalSmooth < t.height)
                    {
                        c += pixels[(x + 0) + ((y + normalSmooth) * t.width)];
                        pixelsToAverage++;
                    }
                    if (x + normalSmooth < t.width)
                    {
                        if (y - normalSmooth > 0)
                        {
                            c += pixels[(x + normalSmooth) + ((y - normalSmooth) * t.width)];
                            pixelsToAverage++;
                        }
                        c += pixels[(x + normalSmooth) + ((y + 0) * t.width)];
                        pixelsToAverage++;
                        if (y + normalSmooth < t.height)
                        {
                            c += pixels[(x + normalSmooth) + ((y + normalSmooth) * t.width)];
                            pixelsToAverage++;
                        }
                    }
                    pixels[x + y * t.width] = c / pixelsToAverage;
                }
            }
        }

        texNormal.SetPixels(pixels);
        texNormal.Apply();
        return texNormal;
    }
    #endregion
}