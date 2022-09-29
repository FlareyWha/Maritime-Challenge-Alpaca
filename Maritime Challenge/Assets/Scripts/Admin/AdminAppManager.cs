using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminAppManager : MonoBehaviour
{
    [SerializeField]
    private GameObject registerPanel, redemptionItemPanel, redemptionPanel;

    [SerializeField]
    private RedemptionItemManager redemptionItemManager;

    [SerializeField]
    private RedemptionRequestManager redemptionRequestManager;

    public void OnRegisterPanelButtonClicked()
    {
        redemptionItemPanel.SetActive(false);
        redemptionPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void OnRedemptionItemPanelButtonClicked()
    {
        registerPanel.SetActive(false);
        redemptionPanel.SetActive(false);
        redemptionItemPanel.SetActive(true);

        redemptionItemManager.GetRedemptionItems();
    }

    public void OnRedemptionPanelButtonClicked()
    {
        registerPanel.SetActive(false);
        redemptionItemPanel.SetActive(false);
        redemptionPanel.SetActive(true);

        redemptionRequestManager.GetRedemptionRequests();
    }
}
