using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailboxUIManager : MonoBehaviourSingleton<MailboxUIManager>
{
    [SerializeField]
    private GameObject MailUIPrefab;
    [SerializeField]
    private Transform MailRect;


    [SerializeField]
    private GameObject EmptyMailboxPanel;


    protected override void Awake()
    {
        base.Awake();

        UpdateMailRect();
    }

    public void UpdateMailRect()
    {
        // Clear
        foreach (Transform child in MailRect)
        {
            Destroy(child.gameObject);
        }
        if (PlayerData.MailList.Count == 0)
        {
            SetEmpty();
            return;
        }

        EmptyMailboxPanel.SetActive(false);

        // Fill
        foreach (Mail mail in PlayerData.MailList)
        {
            MailUI ui = Instantiate(MailUIPrefab, MailRect).GetComponent<MailUI>();
            ui.Init(mail, ClaimMail);
        }

        EmptyMailboxPanel.SetActive(PlayerData.MailList.Count == 0);
    }

    private void SetEmpty()
    {
        EmptyMailboxPanel.SetActive(true);
    }

    private void ClaimMail(MailUI mail)
    {
        // Update Num Tokens
        PlayerData.NumTokens += mail.LinkedMail.MailItemAmount;
        PlayerData.InvokeNumTokensUpdated();
        StartCoroutine(CurrencyManager.DoUpdateTokenAmount());
        // Delete Mail
        MailboxManager.Instance.DeleteMail(mail.LinkedMail);
        // Destroy Mail UI
        Destroy(mail.gameObject);

        // Currency Gain UI
        PopUpManager.Instance.AddCurrencyPopUp(CURRENCY_TYPE.TOKEN, mail.LinkedMail.MailItemAmount, mail.transform.position);

        if (PlayerData.MailList.Count == 0)
            SetEmpty();
    }
}
