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
    private GameObject FriendshipUI;
    [SerializeField]
    private Text FriendshipText;

    private ContactsUI currSelected = null;

    public void UpdateContactsListRect()
    {
        foreach (Transform child in ContactsListRect)
        {
            Destroy(child.gameObject);
        }

        Debug.Log("Updating ContactsListRect...");

        foreach (KeyValuePair<BasicInfo, bool> player in PlayerData.PhonebookData)
        {
            GameObject uiGO = Instantiate(ContactUIPrefab, ContactsListRect);
            ContactsUI contact = uiGO.GetComponent<ContactsUI>();
            if (!player.Value)
            {
                contact.InitUnknown(player.Key, SetSelectedContact);
            }
            else
            {
                contact.Initialise(null, player.Key, SetSelectedContact);
            }
        }

        if (ContactsListRect.childCount > 0)
        {
            currSelected = ContactsListRect.GetChild(0).gameObject.GetComponent<ContactsUI>();
            currSelected.EnableHighlight();
        }
    }

    public void SetSelectedContact(ContactsUI contact)
    {
        if (currSelected != null)
            currSelected.DisableHighlight();
        currSelected = contact;
        if (CheckIfFriends(contact.GetContactInfo().UID))
            SetCurrentFriendInfo(contact.GetContactInfo().UID);
        else
            UpdateContactDisplayUI(contact.GetContactInfo());
    }

    private void UpdateContactDisplayUI(BasicInfo player) // For Unlocked But Not Friends
    {
        FriendshipUI.SetActive(false);
        DisplayNamecard.SetUnknown();
        DisplayName.text = player.Name;
    }

    private void UpdateContactDisplayUI(FriendInfo friend) // For Friends
    {
        FriendshipUI.SetActive(true);
        FriendshipText.text = friend.FriendshipLevel.ToString();
        DisplayNamecard.SetDetails(friend);
        DisplayName.text = friend.Name;
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


    private bool CheckIfFriends(int id)
    {
        Debug.Log("Checking through Friends List...");
        foreach (BasicInfo player in PlayerData.FriendList)
        {
            Debug.Log("Found Friend Name: " + player.Name);
            if (player.UID == id)
                return true;
        }
        return false;
    }

}
