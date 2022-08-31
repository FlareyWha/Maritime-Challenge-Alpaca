using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessageUI : MonoBehaviour
{

    [SerializeField]
    private Text MessageText;

    public void Init(CHAT_TYPE type, string who, string message)
    {
        string typeText = "";
        switch (type)
        {
            case CHAT_TYPE.GUILD:
                typeText = "Guild";
                break;
            case CHAT_TYPE.WORLD:
                typeText = "World";
                break;
        }

        MessageText.text = "[" + typeText +  "] " + who + ": " + message;
    }
}
