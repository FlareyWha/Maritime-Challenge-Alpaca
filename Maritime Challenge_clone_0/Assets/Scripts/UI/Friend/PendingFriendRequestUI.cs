using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PendingFriendRequestUI : MonoBehaviour
{
    [SerializeField]
    private Text RecepientName;
    [SerializeField]
    private Button CancelButton;
   
    private int ReceipentID = 0;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnViewProfile);
        CancelButton.onClick.AddListener(OnCancelButtonClicked);
    }

    public void Init(int rec_id)
    {
        ReceipentID = rec_id;
        RecepientName.text = PlayerData.FindPlayerNameByID(rec_id);
    }

    private void OnCancelButtonClicked()
    {
        FriendsManager.Instance.DeleteFriendRequest(PlayerData.UID, ReceipentID);
        Destroy(gameObject);
    }

    private void OnViewProfile()
    {

    }
}
