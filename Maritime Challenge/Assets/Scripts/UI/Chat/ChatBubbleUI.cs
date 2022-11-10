using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubbleUI : MonoBehaviour
{
    [SerializeField]
    private Text MessageText;

    private float timer = 4.0f;
    private float fade_timer = 0.5f;

    private Color32 MyColor = new Color32(255, 255, 240, 255);

    public void Init(bool isMine, string message)
    {
        if (isMine)
            GetComponent<Image>().color = MyColor;

        MessageText.text = message;

        UIManager.SetHeightByTextHeight(gameObject, MessageText);
    }

    public float GetTimer()
    {
        return timer;
    }

    public void UpdateTimer()
    {
        timer -= Time.deltaTime;
    }

    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();

        float fade_rate = 1.0f / fade_timer;
        while (fade_timer > 0.0f)
        {
            fade_timer -= Time.deltaTime;
            canvasGroup.alpha -= fade_rate * Time.deltaTime;

            yield return null;
        }

        Destroy(gameObject);
    }

}

