using UnityEngine;

public class AllIn1PixelLightCount : MonoBehaviour
{
    public int lightCount = 8;
    void Start()
    {
        QualitySettings.pixelLightCount = lightCount;
    }
}