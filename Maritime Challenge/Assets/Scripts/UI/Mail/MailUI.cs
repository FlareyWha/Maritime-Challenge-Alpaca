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


    private void Awake()
    {
        ClaimButton.onClick.AddListener(OnClaimButtonClicked);

     
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

    public void SetColor(Color color)
    {
        BackgroundImage.color = color;
    }
}
