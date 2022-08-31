using System;
using UnityEngine;
using UnityEngine.UI;

public class ContactsUI : MonoBehaviour
{
    [SerializeField]
    private Image AvatarImage;
    [SerializeField]
    private Text Name;

    private Button button;
    private PlayerInfo linkedPlayer;

    private event Action<ContactsUI> SetSelectedContact;

    void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        SetSelectedContact?.Invoke(this);
    }

    public void Initialise(Sprite avatarSprite, PlayerInfo player, string name, Action<ContactsUI> action)
    {
        AvatarImage.sprite = avatarSprite;
        Name.text = name;
        linkedPlayer = player;
        SetSelectedContact = action;
    }

    public void InitUnknown(Action<ContactsUI> action)
    {
        SetSelectedContact = action;
    }

    public PlayerInfo GetLinkedPlayer()
    {
        return linkedPlayer;
    }


}
