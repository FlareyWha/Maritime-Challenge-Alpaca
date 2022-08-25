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
    private Transform ChatLogRect;

    private PlayerCommands myPlayerCommands = null;

    private CHAT_TYPE chatType = CHAT_TYPE.WORLD;

    void Start()
    {
        StartCoroutine(ChatManagerInits());
    }

    IEnumerator ChatManagerInits()
    {
        while (PlayerData.MyPlayer == null)
            yield return null;

        myPlayerCommands = PlayerData.MyPlayer.gameObject.GetComponent<PlayerCommands>();
    }

    public void SendMessage()
    {
        // Verify not empty
        if (InputField_Message.text == "")
            return;

        // Send Message
        myPlayerCommands.SendChatMessage(InputField_Message.text);
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
