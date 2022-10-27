using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialNPC : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer BodyReferenceSprite;
    [SerializeField]
    private Text PlayerDisplayName;


    private bool isInteractOpen = false;

    private bool interacted = false;
    public bool Interacted
    {
        get { return interacted; }
        private set { }
    }

    [SerializeField]
    private Image InteractPanel;

    [SerializeField]
    private GameObject ChatBubbleUIPrefab;
    [SerializeField]
    private Transform ChatBubbleRect;
    private List<ChatBubbleUI> chatBubbleList = new List<ChatBubbleUI>();

    private void Awake()
    {
    }

    void Update()
    {
        if (InputManager.InputActions.Main.Tap.WasPressedThisFrame() && SpriteHandler.IsWithinSprite(transform.position, BodyReferenceSprite))
            OnPlayerClicked();

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

    private void OnPlayerClicked()
    {
        Debug.Log("Player Entity Click Called");
        OpenInteractPanel();
    }

    public void SetDisplayName(string name)
    {
        PlayerDisplayName.text = name;
    }

    public void AddChatBubble(string text)
    {
        GameObject bubbleGO = Instantiate(ChatBubbleUIPrefab, ChatBubbleRect);
        ChatBubbleUI chatUI = bubbleGO.GetComponent<ChatBubbleUI>();
        chatUI.Init(GetComponent<Player>() == PlayerData.MyPlayer, text);
        chatBubbleList.Add(chatUI);

        // Limit Chat Bubbles
    }



    // OTHER CLIENTS INTERACT WITH LOCAL PLAYER FOR VIEW MENU
    public void OpenInteractPanel()
    {
        if (!isInteractOpen)
        {
            isInteractOpen = true;
            ShowInteractPanel();
        }
    }


    public void CloseInteractPanel()
    {
        isInteractOpen = false;
        HideInteractPanel();
    }
    public void ShowInteractPanel()
    {
        StartCoroutine(UIManager.ToggleSlideAnim(InteractPanel, true, 0.6f, null));
    }

    public void HideInteractPanel()
    {
        StartCoroutine(UIManager.ToggleSlideAnim(InteractPanel, false, 0.2f, null));
    }

   
    public void ViewProfile(Button button)
    {
        Debug.Log("View Profile Clicked");

     
        UIManager.Instance.ShowInteractNamecard(button);

        interacted = true;
    }

   
}
