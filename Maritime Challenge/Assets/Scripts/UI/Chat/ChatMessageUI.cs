using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChatMessageUI : MonoBehaviour
{

    [SerializeField]
    private Text MessageText;

    private Color32 myMessageColor = new Color32(0, 40, 100, 255);

    public void Init(CHAT_TYPE type, int sender_id, string who, string message)
    {
        if (who == "")
            who = "Guest";
        else if (sender_id == PlayerData.MyPlayer.GetUID())
            MessageText.color = myMessageColor;

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
