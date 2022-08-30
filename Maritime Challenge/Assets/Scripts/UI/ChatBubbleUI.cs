using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubbleUI : MonoBehaviour
{
    [SerializeField]
    private Text MessageText;

    private float timer = 5.0f;
    private float fade_timer = 0.5f;

    public void Init(string message)
    {
        MessageText.text = message;



        float text_height = MessageText.preferredHeight;
        Debug.Log("Text Height: " + text_height);
        RectTransform rt = GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(rt.rect.width, text_height);
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

