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
    private Image DisplayAvatar;
    [SerializeField]
    private GameObject FriendshipUI;
    [SerializeField]
    private Text FriendshipText;

    private ContactsUI currSelected = null;

    private void Start()
    {
        FriendsManager.OnFriendListUpdated += UpdateDisplay;
        FriendsManager.OnNewFriendDataSaved += OnFriendDataSaved;
        FriendRequestHandler.OnFriendRequestSent += OnFriendRequestsUpdated;
        FriendRequestHandler.OnFriendRequestDeleted += OnFriendRequestsUpdated;
    }

    public void UpdateContactsListRect()
    {
        foreach (Transform child in ContactsListRect)
        {
            Destroy(child.gameObject);
        }


        // TBC - Sort List By Known/Unknown

        foreach (KeyValuePair<int, BasicInfo> player in PlayerData.PhonebookData)
        {
            GameObject uiGO = Instantiate(ContactUIPrefab, ContactsListRect);
            ContactsUI contact = uiGO.GetComponent<ContactsUI>();
            if (!player.Value.Unlocked)
            {
                contact.InitUnknown(player.Value, SetSelectedContact);
            }
            else
            {
                contact.Initialise(null, player.Value, SetSelectedContact);
            }

            if (currSelected == null)
            {
                SetSelectedContact(contact);
                currSelected.EnableHighlight();
            }
        }

     
    }

    public void SetSelectedContact(ContactsUI contact)
    {
        if (currSelected != null)
            currSelected.DisableHighlight();
        currSelected = contact;
        UpdateDisplay();
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
    }

    private void UpdateContactDisplayUI(BasicInfo player) // For Unlocked But Not Friends
    {
        FriendshipUI.SetActive(false);
        DisplayNamecard.SetHidden(currSelected.GetContactInfo().UID);
        DisplayName.text = player.Name;
    }

    private void UpdateContactDisplayUI(FriendInfo friend) // For Friends
    {
        FriendshipUI.SetActive(true);
        FriendshipText.text = friend.FriendshipLevel.ToString();
        DisplayNamecard.SetDetails(friend);
        DisplayName.text = friend.Name;
    }

    private void HideContactDisplayInfo() // For Not Unlocked
    {
        FriendshipUI.SetActive(false);
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
