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

    private FriendInfo currSelected;

    private void UpdateContactsListDisplay()
    {
        foreach (Transform child in ContactsListRect)
        {
            Destroy(child.gameObject);
        }


        //foreach (Player player in PlayerData.FriendList)
        //{
        //      
        //}
    }

    public void SetSelectedContact(ContactsUI contact)
    {
        currSelected = contact.GetLinkedPlayer();
        UpdateContactDisplayUI();
    }

    private void UpdateContactDisplayUI()
    {

    }

    private void GetFriendInfo(int friendUID)
    {
        StartCoroutine(StartGetFriendInfo(friendUID));
    }

    IEnumerator StartGetFriendInfo(int friendUID)
    {
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
                JSONDeseralizer.DeseralizeFriendData(friendUID, webreq.downloadHandler.text);

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
