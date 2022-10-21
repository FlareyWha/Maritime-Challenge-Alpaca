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

    private Mail LinkedMail;
    private Action<Mail> onClaimButtonClicked;

    private Color32[] BackgroundColorOptions =
    {
        new Color32(200, 200, 255, 255),
        new Color32(200, 255, 200, 255),
        new Color32(255, 200, 200, 255)
    };

    private void Awake()
    {
        ClaimButton.onClick.AddListener(OnClaimButtonClicked);

        // Set Random BG Color
        int randOption = UnityEngine.Random.Range(0, BackgroundColorOptions.Length - 1);
        BackgroundImage.color = BackgroundColorOptions[randOption];
    }

    public void Init(Mail mail, Action<Mail> action)
    {
        onClaimButtonClicked = action;

        LinkedMail = mail;

        MessageText.text = mail.MailTitle;
    }

    private void OnClaimButtonClicked()
    {
        onClaimButtonClicked?.Invoke(LinkedMail);
    }
}
