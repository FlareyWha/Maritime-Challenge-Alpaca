using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncomingFriendRequestUI : MonoBehaviour
{
    [SerializeField]
    private Text SenderName;
    [SerializeField]
    private Button AcceptButton, RejectButton;

    private int SenderID = 0;

    void Start()
    {
        AcceptButton.onClick.AddListener(OnAcceptButtonClicked);
        RejectButton.onClick.AddListener(OnRejectButtonClicked);
        gameObject.GetComponent<Button>().onClick.AddListener(OnViewProfile);
    }

    public void Init(int sender_id)
    {
        SenderID = sender_id;
        SenderName.text = PlayerData.FindPlayerNameByID(sender_id);
    }

    private void OnAcceptButtonClicked()
    {
        FriendsManager.Instance.AddFriend(SenderID, SenderName.text);
        FriendRequestHandler.InvokeFriendRequestDeletedEvent(SenderID, PlayerData.UID);
        Destroy(gameObject);
    }

    private void OnRejectButtonClicked()
    {
        FriendsManager.Instance.DeleteFriendRequest(SenderID, PlayerData.UID);
        Destroy(gameObject);
    }

    private void OnViewProfile()
    {
        
    }
}
