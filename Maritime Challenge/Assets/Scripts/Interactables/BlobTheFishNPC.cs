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

    // Start is called before the first frame update
    void Start()
    {
        interactMessage = "Speak to BlobTheFish";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        // Chat Bubble
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
    }
}
