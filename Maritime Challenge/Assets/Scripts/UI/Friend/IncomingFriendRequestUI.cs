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

    private event Action<int> acceptFriendRequest, rejectFriendRequest;

    void Start()
    {
        AcceptButton.onClick.AddListener(OnAcceptButtonClicked);
        RejectButton.onClick.AddListener(OnRejectButtonClicked);
    }

    public void Init()
    {
        
    }

    private void OnAcceptButtonClicked()
    {
        acceptFriendRequest?.Invoke(SenderID);
    }

    private void OnRejectButtonClicked()
    {
        rejectFriendRequest?.Invoke(SenderID);
    }
}
