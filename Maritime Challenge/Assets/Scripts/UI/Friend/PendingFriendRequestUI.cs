using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PendingFriendRequestUI : MonoBehaviour
{
    [SerializeField]
    private Text RecepientName;
   
    private int ReceipentID = 0;


    public void Init(int rec_id)
    {
        ReceipentID = rec_id;
        RecepientName.text = PlayerData.FindPlayerNameByID(rec_id);
    }
}
