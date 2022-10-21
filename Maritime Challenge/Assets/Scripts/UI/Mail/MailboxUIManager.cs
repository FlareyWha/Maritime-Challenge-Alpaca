using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MailboxUIManager : MonoBehaviour
{
    [SerializeField]
    private GameObject MailUIPrefab;
    [SerializeField]
    private Transform MailRect;


    [SerializeField]
    private GameObject EmptyMailboxPanel;


    private void Awake()
    {
        UpdateMailRect();
    }

    private void UpdateMailRect()
    {
        // Clear
        foreach (Transform child in MailRect)
        {
            Destroy(child.gameObject);
        }
        // Fill
        foreach (Mail mail in PlayerData.MailList)
        {
            MailUI ui = Instantiate(MailUIPrefab, MailRect).GetComponent<MailUI>();
            ui.Init(mail, ClaimMail);
        }

        EmptyMailboxPanel.SetActive(PlayerData.MailList.Count == 0);
    }

    private void ClaimMail(Mail mail)
    {

    }
}
