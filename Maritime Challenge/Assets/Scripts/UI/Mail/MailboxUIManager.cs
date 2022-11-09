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

    private Color32[] BackgroundColorOptions =
   {
        new Color32(255, 206, 199, 255),
        new Color32(255, 222, 199, 255),
        new Color32(255, 252, 199, 255),
        new Color32(232, 255, 199, 255),
        new Color32(199, 230, 255, 255),
        new Color32(199, 207, 255, 255),
        new Color32(205, 199, 255, 255),
    };

    protected override void Awake()
    {
        base.Awake();
        UpdateMailRect();
    }

    public void UpdateMailRect()
    {
        // Clear - but retain color first
        Dictionary<int, Color> prevList = new Dictionary<int, Color>();
        foreach (Transform child in MailRect)
        {
            MailUI ui = child.gameObject.GetComponent<MailUI>();
            prevList.Add(ui.LinkedMail.MailID, ui.GetComponent<Image>().color);
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

            bool isNew = true;
            foreach (KeyValuePair<int, Color> prevMail in prevList)
            {
                if (mail.MailID == prevMail.Key)
                {
                    isNew = false;
                    ui.SetColor(prevMail.Value);
                    break;
                }
            }
            if (isNew)
            {
                // Set Random BG Color
                int randOption = UnityEngine.Random.Range(0, BackgroundColorOptions.Length - 1);
                ui.SetColor(BackgroundColorOptions[randOption]);
            }


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
