using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
 
    [SerializeField]
    private Text PlayerDisplayName;


    [SerializeField]
    private Image InteractPanel;

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

    public void ShowInteractPanel()
    {
        StartCoroutine(UIManager.ToggleSlideAnim(InteractPanel, true, 0.6f, null));
    }

    public void HideInteractPanel()
    {
        StartCoroutine(UIManager.ToggleSlideAnim(InteractPanel, false, 0.2f, null));
    }

    void FixedUpdate()
    {
        List<ChatBubbleUI> toRemoveList = new List<ChatBubbleUI>();
        foreach (ChatBubbleUI bubble in chatBubbleList)
        {
            ChatBubbleUI oldest = chatBubbleList[0];
            oldest.UpdateTimer();
            
            if (oldest.GetTimer() <= 0.0f)
            {
                toRemoveList.Add(oldest);
                oldest.StartFadeOut();
            }
        }
        foreach (ChatBubbleUI bubble in toRemoveList)
        {
            chatBubbleList.Remove(bubble);
        }

    }
}
