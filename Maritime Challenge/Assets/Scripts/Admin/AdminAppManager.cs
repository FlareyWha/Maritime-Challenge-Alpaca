using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AdminAppManager : MonoBehaviour
{
    [SerializeField]
    private GameObject registerPanel, refreshDatabasePanel, redemptionItemPanel, redemptionPanel;

    [SerializeField]
    private RefreshDatabaseManager refreshDatabaseManager;

    [SerializeField]
    private RedemptionItemManager redemptionItemManager;

    [SerializeField]
    private RedemptionRequestManager redemptionRequestManager;

    public void OnRegisterPanelButtonClicked()
    {
        refreshDatabasePanel.SetActive(false);
        redemptionItemPanel.SetActive(false);
        redemptionPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

    public void OnRefreshDatabasePanelButtonClicked()
    {
        registerPanel.SetActive(false);
        redemptionItemPanel.SetActive(false);
        redemptionPanel.SetActive(false);
        refreshDatabasePanel.SetActive(true);

        refreshDatabaseManager.RefreshDatabase();
    }

    public void OnRedemptionItemPanelButtonClicked()
    {
        registerPanel.SetActive(false);
        refreshDatabasePanel.SetActive(false);
        redemptionPanel.SetActive(false);
        redemptionItemPanel.SetActive(true);

        redemptionItemManager.GetRedemptionItems();
    }

    public void OnRedemptionPanelButtonClicked()
    {
        registerPanel.SetActive(false);
        refreshDatabasePanel.SetActive(false);
        redemptionItemPanel.SetActive(false);
        redemptionPanel.SetActive(true);

        redemptionRequestManager.GetRedemptionRequests();
    }
}
