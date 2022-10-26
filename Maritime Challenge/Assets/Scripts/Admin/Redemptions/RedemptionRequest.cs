using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedemptionRequest : MonoBehaviour
{
    private List<int> redemptionRequestID = new List<int>();

    [SerializeField]
    private Text usernameText, redemptionItemNameText, amountText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitRedemptionRequest(List<int> ID, string username, string redemptionItemName, int amount)
    {
        redemptionRequestID = ID;
        usernameText.text = username;
        redemptionItemNameText.text = redemptionItemName;
        amountText.text = "x" + amount;
    }
}
