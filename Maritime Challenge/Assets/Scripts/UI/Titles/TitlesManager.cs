using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TitlesManager : MonoBehaviour
{
    [SerializeField]
    private GameObject TitleUIPrefab;
    [SerializeField]
    private Transform TitlesRect;

    [SerializeField]
    private Image ProfileDisplayTitle;


    private TitleUI currSelected;

    void Start()
    {
        UpdateTitlesRect();
    }

    private void UpdateTitlesRect()
    {
        foreach (Transform child in TitlesRect)
        {
            Destroy(child.gameObject);
        }


        foreach (KeyValuePair<Title, bool> title in PlayerData.titleDictionary)
        {
            TitleUI titleUI = Instantiate(TitleUIPrefab, TitlesRect).GetComponent<TitleUI>();
            titleUI.Init(title.Key, title.Value, SwitchTitle);

            // If Is Currently Equipped
            if (PlayerData.CurrentTitleID == title.Key.titleID)
            {
                currSelected = titleUI;
                currSelected.ToggleEquippedOverlay(true);
            }
        }
    }

    private void SwitchTitle(TitleUI newTitle)
    {
        currSelected.ToggleEquippedOverlay(false);
        currSelected = newTitle;

        EditTitle();

        ProfileDisplayTitle.sprite = newTitle.LinkedTitle.LinkedTitle.TitleSprite;
    }

    public void EditTitle()
    {
        StartCoroutine(StartEditTitle());
    }

    IEnumerator StartEditTitle()
    {
        string url = ServerDataManager.URL_updateTitle;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iCurrentTitleID", currSelected.LinkedTitle.titleID); //Change later btw
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                Debug.Log(webreq.downloadHandler.text);
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
