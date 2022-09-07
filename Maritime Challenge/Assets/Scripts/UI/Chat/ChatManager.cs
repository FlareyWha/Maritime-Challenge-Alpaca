using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ChatManager : MonoBehaviourSingleton<ChatManager>
{

    [SerializeField]
    private GameObject ChatMessageUIPrefab;
    [SerializeField]
    private InputField InputField_Message;
    [SerializeField]
    private Dropdown Dropdown_ChatType;
    [SerializeField]
    private Transform ChatLogRect;


    private CHAT_TYPE chatType = CHAT_TYPE.WORLD;
    private List<string> chatTypeList = new List<string>() { "World", "Guild" };

    void Start()
    {
        StartCoroutine(ChatManagerInits());
    }

    IEnumerator ChatManagerInits()
    {

        Dropdown_ChatType.ClearOptions();
        Dropdown_ChatType.AddOptions(chatTypeList);


        while (PlayerData.MyPlayer == null)
            yield return null;

    }

    public void SendMessage()
    {
        // Verify not empty
        if (InputField_Message.text == "")
            return;

        // Send Message
        PlayerData.CommandsHandler.SendChatMessage(InputField_Message.text);
        InputField_Message.text = "";
    }

    public void UpdateChatLog(CHAT_TYPE chat_type, string sender_name, string message)
    {
        GameObject go = Instantiate(ChatMessageUIPrefab, ChatLogRect);
        ChatMessageUI chatUI = go.GetComponent<ChatMessageUI>();
        chatUI.Init(chat_type, sender_name, message);
    }

    public void OnChatTypeChanged()
    {
        chatType = (CHAT_TYPE)Dropdown_ChatType.value;
    }

    public CHAT_TYPE GetChatType()
    {
        return chatType;
    }
}

public enum CHAT_TYPE
{
    WORLD,
    GUILD,

    NUM_TOTAL
}
