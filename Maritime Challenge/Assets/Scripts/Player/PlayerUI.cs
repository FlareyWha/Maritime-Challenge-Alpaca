using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
 
    [SerializeField]
    private Text PlayerDisplayName;

    [SerializeField]
    private GameObject ChatBubbleUIPrefab;
    [SerializeField]
    private Transform ChatBubbleRect;
    private List<ChatBubbleUI> chatBubbleList = new List<ChatBubbleUI>();


    public void SetDisplayName(string name)
    {
        PlayerDisplayName.text = name;
    }

    public void AddChatBubble(string text)
    {
        GameObject bubbleGO = Instantiate(ChatBubbleUIPrefab, ChatBubbleRect);
        ChatBubbleUI chatUI = bubbleGO.GetComponent<ChatBubbleUI>();
        chatUI.Init(text);
        chatBubbleList.Add(chatUI);

        // Limit Chat Bubbles
    }
}
