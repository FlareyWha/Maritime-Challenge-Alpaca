using System;
using UnityEngine;
using UnityEngine.UI;

public class MailUI : MonoBehaviour
{
    [SerializeField]
    private Image BackgroundImage;
    [SerializeField]
    private Text MessageText;
    [SerializeField]
    private Button ClaimButton;

    public Mail LinkedMail;
    private Action<MailUI> onClaimButtonClicked;

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

    private void Awake()
    {
        ClaimButton.onClick.AddListener(OnClaimButtonClicked);

        // Set Random BG Color
        int randOption = UnityEngine.Random.Range(0, BackgroundColorOptions.Length - 1);
        BackgroundImage.color = BackgroundColorOptions[randOption];
    }

    public void Init(Mail mail, Action<MailUI> action)
    {
        onClaimButtonClicked = action;

        LinkedMail = mail;

        MessageText.text = mail.MailTitle;
    }

    private void OnClaimButtonClicked()
    {
        ClaimButton.interactable = false;
        onClaimButtonClicked?.Invoke(this);
    }
}
