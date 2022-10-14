using System;
using UnityEngine;
using UnityEngine.UI;

public class ContactsUI : MonoBehaviour
{
    [SerializeField]
    private AvatarDisplay DisplayAvatar;
    [SerializeField]
    private Image Highlight;
    [SerializeField]
    private Text Name;
    [SerializeField]
    private Sprite UnknownSprite;

    private Button button;
    private BasicInfo contactInfo;

    private event Action<ContactsUI> SetSelectedContact;

    private bool is_unlocked = false;

    void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    private void OnButtonClicked()
    {
        SetSelectedContact?.Invoke(this);
        EnableHighlight();
    }

    public void Initialise(BasicInfo contact, Action<ContactsUI> action)
    {
        DisplayAvatar.SetFromInfo(contact);

        Name.text = contact.Name;
        contactInfo = contact;
        SetSelectedContact = action;

        is_unlocked = true;
    }

    public void InitUnknown(BasicInfo contact, Action<ContactsUI> action)
    {
        DisplayAvatar.SetFromInfo(null);

        SetSelectedContact = action;
        contactInfo = contact;
        Name.text = "";

        is_unlocked = false;
    }

    public void DisableHighlight()
    {
        Highlight.color = new Color32(255, 255, 255, 50);
    }

    public void EnableHighlight()
    {

        Highlight.color = new Color32(255, 255, 255, 150);

    }

    public BasicInfo GetContactInfo()
    {
        return contactInfo;
    }

    public bool GetUnlockStatus()
    {
        return is_unlocked;
    }


}
