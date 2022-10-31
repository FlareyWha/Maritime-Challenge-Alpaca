using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Mirror;

public class PlayerUI : MonoBehaviour
{
 
    [SerializeField]
    private Text PlayerDisplayName;


    private bool isInteractOpen = false;
    public static Player interactPlayer = null;

    [SerializeField]
    private Image InteractPanel;

    [SerializeField]
    private GameObject ChatBubbleUIPrefab;
    [SerializeField]
    private Transform ChatBubbleRect;
    private List<ChatBubbleUI> chatBubbleList = new List<ChatBubbleUI>();

    void Start()
    {
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

    public void ViewProfile(Button button)
    {
        Debug.Log("View Profile Clicked");
        Player player = gameObject.GetComponent<Player>();
        PlayerStatsManager.Instance.UpdatePlayerStat(PLAYER_STAT.PROFILES_VIEWED, ++PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.PROFILES_VIEWED]);

        //Unlock the phonebook data if other isnt unlocked to begin with
        if (!PlayerData.PhonebookData[player.GetUID()].Unlocked)
        {
            StartCoroutine(UpdatePhonebookOtherUnlocked(player.GetUID()));
            PlayerStatsManager.Instance.UpdateXPLevels(GameSettings.XP_PER_RIGHTSHIPEDIA_ENTRY);
            PlayerStatsManager.Instance.UpdatePlayerStat(PLAYER_STAT.RIGHTSHIPEDIA_ENTRIES_UNLOCKED, ++PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.RIGHTSHIPEDIA_ENTRIES_UNLOCKED]);
        }
        else
        {
            UIManager.Instance.SetInteractNamecardDetails(player);
        }

        UIManager.Instance.ShowInteractNamecard(button);
        interactPlayer = player;
    }

    public static IEnumerator UpdatePhonebookOtherUnlocked(int playerID)
    {
        string url = ServerDataManager.URL_updatePhonebookOtherUnlocked;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("OtherUID", playerID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                Debug.Log(webreq.downloadHandler.text);
                PlayerData.PhonebookData[playerID].Unlocked = true;

                ContactsManager.Instance.InvokeNewRightShipediaEntryEvent(playerID);

                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

}
