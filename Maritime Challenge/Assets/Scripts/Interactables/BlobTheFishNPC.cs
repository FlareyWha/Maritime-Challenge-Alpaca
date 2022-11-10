using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlobTheFishNPC : BaseInteractable
{
    private bool interacted;
    public bool Interacted
    {
        get { return interacted; }
        private set { }
    }

    [SerializeField]
    private GameObject ChatBubbleUIPrefab;
    [SerializeField]
    private Transform ChatBubbleRect;
    private List<ChatBubbleUI> chatBubbleList = new List<ChatBubbleUI>();

    private const int MaxChatBubbleNum = 4;

    // Start is called before the first frame update
    void Start()
    {
        interactMessage = "Speak to BlobTheFish";
    }

    
    private void FixedUpdate()
    {
        // Chat Bubble
        List<ChatBubbleUI> toRemoveList = new List<ChatBubbleUI>();

        if (chatBubbleList.Count > 0)
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

    public override void Interact()
    {
        interacted = true;

        AddChatBubble("Blub blub blub");
    }

    public void AddChatBubble(string text)
    {
        GameObject bubbleGO = Instantiate(ChatBubbleUIPrefab, ChatBubbleRect);
        ChatBubbleUI chatUI = bubbleGO.GetComponent<ChatBubbleUI>();
        chatUI.Init(GetComponent<Player>() == PlayerData.MyPlayer, text);
        chatBubbleList.Add(chatUI);

        // Limit Chat Bubbles
        if (chatBubbleList.Count > MaxChatBubbleNum)
        {
            ChatBubbleUI ui = chatBubbleList[0];
            chatBubbleList.Remove(ui);
            Destroy(ui.gameObject);
        }
    }
}
