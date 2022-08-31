using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContactsManager : MonoBehaviour
{
    [SerializeField]
    private GameObject ContactUIPrefab;
    [SerializeField]
    private Transform ContactsListRect;

    private PlayerInfo currSelected;

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
}
