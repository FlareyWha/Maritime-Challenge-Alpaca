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
        }

        if (ContactsListRect.childCount > 0)
        {
            // ITS LITERALLY THE SAME WHY NO WORK AAAAAAAAAAAAAAAAA

            //SetSelectedContact(ContactsListRect.GetChild(0).gameObject.GetComponent<ContactsUI>());
            //currSelected.EnableHighlight();
            ContactsListRect.GetChild(0).gameObject.GetComponent<ContactsUI>().OnButtonClicked();
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

        StartCoroutine(StartGetFriendInfo(friendUID));
    }

    IEnumerator StartGetFriendInfo(int friendUID)
    {
        // Add Loading Anim

        string url = ServerDataManager.URL_getFriendInfo;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iFriendUID", friendUID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                FriendInfo friend =  JSONDeseralizer.DeseralizeFriendData(friendUID, webreq.downloadHandler.text);
                UpdateContactDisplayUI(friend);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public void AddSelectedContactAsFriend()
    {
        FriendsManager.Instance.AddFriend(currSelected.GetContactInfo().UID, currSelected.GetContactInfo().Name);
    }

    public void UnfriendContact()
    {
        FriendsManager.Instance.DeleteFriend(currSelected.GetContactInfo().UID);
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
