using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ContactsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ContactUIPrefab;
    [SerializeField]
    private Transform ContactsListRect;
    [SerializeField]
    private ProfileNamecard DisplayNamecard;
    [SerializeField]
    private Text DisplayName;
    [SerializeField]
    private AvatarDisplay DisplayAvatar;

    [SerializeField]
    private GameObject FriendshipUI;
    [SerializeField]
    private Text FriendshipLevelText;
    [SerializeField]
    private Image FriendshipXPFill;

    [SerializeField]
    private Text GiftRemainingNumText;
    [SerializeField]
    private Image GiftButtonIcon;
    [SerializeField]
    private Button GiftButton;
    [SerializeField]
    private GameObject GiftRemainingNumPopUp;
    private Color32 giftDisabledColor = new Color32(160, 160, 160, 200);

    private ContactsUI currSelected = null;

    private void Start()
    {
        FriendsManager.OnFriendListUpdated += UpdateDisplay;
        FriendsManager.OnNewFriendDataSaved += OnFriendDataSaved;
        FriendRequestHandler.OnFriendRequestSent += OnFriendRequestsUpdated;
        FriendRequestHandler.OnFriendRequestDeleted += OnFriendRequestsUpdated;
        UpdateDisplay();
        UpdateContactsListRect();
        UpdateGiftUI();
    }

    public void UpdateContactsListRect()
    {
        foreach (Transform child in ContactsListRect)
        {
            Destroy(child.gameObject);
        }


        List<ContactsUI> contactsList = new List<ContactsUI>();
        foreach (KeyValuePair<int, BasicInfo> player in PlayerData.PhonebookData)
        {
            GameObject uiGO = Instantiate(ContactUIPrefab);
            ContactsUI contact = uiGO.GetComponent<ContactsUI>();
            if (!player.Value.Unlocked)
            {
                contact.InitUnknown(player.Value, SetSelectedContact);
                contact.SortOrderRef = -1;
            }
            else
            {
                contact.Initialise(player.Value, SetSelectedContact);
                contact.SortOrderRef = 1;
            }

            if (currSelected == null)
            {
                SetSelectedContact(contact);
                if (!player.Value.Unlocked)
                    DisplayAvatar.SetUnknown();
                else
                    DisplayAvatar.SetPlayer(player.Value.UID);
                currSelected.EnableHighlight();
            }
            contactsList.Add(contact);
        }
        // Sort List By Known/Unknown
        contactsList.Sort((a, b) => { return b.SortOrderRef.CompareTo(a.SortOrderRef); });
        foreach (ContactsUI ui in contactsList)
        {
            ui.gameObject.transform.SetParent(ContactsListRect);
            ui.gameObject.transform.localScale = Vector3.one;
        }

    }

    public void SetSelectedContact(ContactsUI contact)
    {
        if (currSelected != null)
            currSelected.DisableHighlight();
        currSelected = contact;
       
        UpdateDisplay();
    }


    public void OnGiftButtonClicked()
    {
        MailboxManager.Instance.SendFriendshipGiftMail(currSelected.GetContactInfo().UID, 20);
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.GIFTS_SENT_DAILY]++;
        PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.GIFTS_SENT_WEEKLY]++;
        UpdateGiftUI();
    }

    private void UpdateGiftUI()
    {
        int remainingNum = GameSettings.NumGiftsDaily - PlayerData.PlayerStats.PlayerStat[(int)PLAYER_STAT.GIFTS_SENT_DAILY];
        GiftRemainingNumText.text = remainingNum.ToString();
        if (remainingNum == 0)
        {
            GiftButtonIcon.color = giftDisabledColor;
            GiftRemainingNumPopUp.gameObject.SetActive(false);
            GiftButton.interactable = false;
        }
    }

    private void UpdateDisplay()
    {
        if (currSelected == null)
            return;

        if (FriendsManager.CheckIfFriends(currSelected.GetContactInfo().UID))
            SetCurrentFriendInfo(currSelected.GetContactInfo().UID);
        else if (currSelected.GetUnlockStatus())
            UpdateContactDisplayUI(currSelected.GetContactInfo());
        else
            HideContactDisplayInfo();


        if (currSelected.GetUnlockStatus())
            DisplayAvatar.SetPlayer(currSelected.GetContactInfo().UID);
        else
            DisplayAvatar.SetUnknown();
    }

    private void UpdateContactDisplayUI(BasicInfo player) // For Unlocked But Not Friends
    {
        FriendshipUI.SetActive(false);
        DisplayAvatar.SetPlayer(player.UID);
        DisplayNamecard.SetHidden(player);
        DisplayName.text = player.Name;
    }

    private void UpdateContactDisplayUI(FriendInfo friend) // For Friends
    {
        FriendshipUI.SetActive(true);
        FriendshipLevelText.text = friend.FriendshipLevel.ToString();
        FriendshipXPFill.fillAmount = (float)friend.FriendshipXP / GameSettings.GetFriendshipXPRequirement(friend.FriendshipLevel);
        DisplayAvatar.SetPlayer(friend.UID);
        DisplayNamecard.SetDetails(friend);
        DisplayName.text = friend.Name;
    }

    private void HideContactDisplayInfo() // For Not Unlocked
    {
        FriendshipUI.SetActive(false);
        DisplayAvatar.SetUnknown();
        DisplayNamecard.SetUnknown(currSelected.GetContactInfo().UID);
        DisplayName.text = "?";
    }

    private void SetCurrentFriendInfo(int friendUID)
    {
        foreach (FriendInfo friend in PlayerData.FriendDataList)
        {
            if (friend.UID == friendUID)
            {
                UpdateContactDisplayUI(friend);
                return;
            }
        }

        FriendsManager.Instance.GetFriendDataInfo(friendUID);
    }

    

    //public void AddSelectedContactAsFriend()
    //{
        
    //}

    //public void UnfriendContact()
    //{
    //    FriendsManager.Instance.DeleteFriend(currSelected.GetContactInfo().UID);
    //}


    private void OnFriendRequestsUpdated(int sender_id, int rec_id)
    {
        if (currSelected == null)
            return;

        int currDisplayID = currSelected.GetContactInfo().UID;

        if (currDisplayID == sender_id || currDisplayID == rec_id)
            UpdateDisplay();
    }

    private void OnFriendDataSaved(FriendInfo friend)
    {
        if (currSelected.GetContactInfo().UID == friend.UID)
            UpdateContactDisplayUI(friend);
    }


    public static bool CheckIfKnown(int playerID)
    {
        foreach (KeyValuePair<int, BasicInfo> player in PlayerData.PhonebookData)
        {
            if (player.Value.UID == playerID)
                return player.Value.Unlocked;
        }
        return false;
    }

}
