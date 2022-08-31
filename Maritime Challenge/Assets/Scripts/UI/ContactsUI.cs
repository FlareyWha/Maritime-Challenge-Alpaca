using System;
using UnityEngine;
using UnityEngine.UI;

public class ContactsUI : MonoBehaviour
{
    [SerializeField]
    private Image AvatarImage, Highlight;
    [SerializeField]
    private Text Name;

    private Button button;
    private int linkedPlayerID;

    private event Action<ContactsUI> SetSelectedContact;

    void Awake()
    {
        button = gameObject.GetComponent<Button>();
        button.onClick.AddListener(OnButtonClicked);
    }

    void OnButtonClicked()
    {
        SetSelectedContact?.Invoke(this);
        EnableHighlight();
    }

    public void Initialise(Sprite avatarSprite, int playerID, string name, Action<ContactsUI> action)
    {
        AvatarImage.sprite = avatarSprite;
        Name.text = name;
        linkedPlayerID = playerID;
        SetSelectedContact = action;
    }

    public void InitUnknown(Action<ContactsUI> action)
    {
        SetSelectedContact = action;
    }

    public void DisableHighlight()
    {
        Highlight.color = new Color32(255, 255, 255, 50);
    }

    private void EnableHighlight()
    {

        Highlight.color = new Color32(255, 255, 255, 150);

    }

    public int GetLinkedPlayerID()
    {
        return linkedPlayerID;
    }


}
