using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PendingFriendRequestUI : MonoBehaviour
{
    [SerializeField]
    private Text RecepientName;
   
    private int ReceipentID = 0;

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnViewProfile);
    }

    public void Init(int rec_id)
    {
        ReceipentID = rec_id;
        RecepientName.text = PlayerData.FindPlayerNameByID(rec_id);
    }

    private void OnViewProfile()
    {

    }
}
