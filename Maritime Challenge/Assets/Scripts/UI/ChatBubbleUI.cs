using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatBubbleUI : MonoBehaviour
{
    [SerializeField]
    private Text MessageText;

    public void Init(string message)
    {
        MessageText.text = message;
    }
}
