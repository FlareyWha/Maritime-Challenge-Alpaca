using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RedemptionManager : MonoBehaviour
{
    [SerializeField]
    private List<RedeemableItemSO> RedeemableItemsList;

    [SerializeField]
    private GameObject RedemptionItemUIPrefab;
    [SerializeField]
    private Transform VouchersRect;

    [SerializeField]
    private Text NumRollarsText;

    private void Awake()
    {
        UpdateNumRollarsUI();
        PlayerData.OnNumRollarsUpdated += UpdateNumRollarsUI;

        // Clear
        foreach (Transform child in VouchersRect)
        {
            Destroy(child.gameObject);
        }
        foreach (RedeemableItemSO item in RedeemableItemsList)
        {
            RedemptionItemUI redeemUI = Instantiate(RedemptionItemUIPrefab, VouchersRect).GetComponent<RedemptionItemUI>();
            redeemUI.Init(item, ExchangeItem);
        }
    }

    private void OnDestroy()
    {
         PlayerData.OnNumRollarsUpdated -= UpdateNumRollarsUI;
    }

    private void ExchangeItem(RedemptionItemUI item)
    {
        if (PlayerData.NumRightshipRollars < item.Item.RollarsCost)
        {
            // ui not enough rollars
            return;
        }

        // Redeem Logic
        AddRedemptionRequest(item.Item.ID);
        // Currency Change
        CurrencyManager.Instance.UpdateRightShipRollarsAmount(-1 * item.Item.RollarsCost);
        PopUpManager.Instance.AddCurrencyPopUp(CURRENCY_TYPE.ROLLAR, -1 * item.Item.RollarsCost, item.transform.position);
    }

    private void UpdateNumRollarsUI()
    {
        NumRollarsText.text = PlayerData.NumRightshipRollars.ToString();
    }


    public void AddRedemptionRequest(int redemptionItemID)
    {
        StartCoroutine(DoAddRedemptionRequest(redemptionItemID));
    }

    IEnumerator DoAddRedemptionRequest(int redemptionItemID)
    {
        string url = ServerDataManager.URL_getRedemptionRequests;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iRedemptionItemID", redemptionItemID);
        form.AddField("iOwnerUID", PlayerData.UID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError(webreq.downloadHandler.text);
                break;
        }
    }
}
