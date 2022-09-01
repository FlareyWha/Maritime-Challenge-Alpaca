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

    private ContactsUI currSelected;

    public void UpdateContactsListRect()
    {
        foreach (Transform child in ContactsListRect)
        {
            Destroy(child.gameObject);
        }


        foreach (KeyValuePair<BasicInfo, bool> player in PlayerData.PhonebookData)
        {
            GameObject uiGO = Instantiate(ContactUIPrefab, ContactsListRect);
            ContactsUI contact = uiGO.GetComponent<ContactsUI>();
            if (!player.Value)
            {
                contact.InitUnknown(SetSelectedContact);
            }
            else
            {
                contact.Initialise(null, player.Key.UID, player.Key.Name, SetSelectedContact);
            }
        }
    }

    public void SetSelectedContact(ContactsUI contact)
    {
        currSelected.DisableHighlight();
        currSelected = contact;
        SetCurrentFriendInfo(contact.GetLinkedPlayerID());
    }

    private void UpdateContactDisplayUI(FriendInfo friend)
    {
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


        //Should this be above
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
}
