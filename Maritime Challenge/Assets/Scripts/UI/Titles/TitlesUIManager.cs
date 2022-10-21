using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TitlesUIManager : MonoBehaviourSingleton<TitlesUIManager>
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

    public void UpdateTitlesRect()
    {
        foreach (Transform child in TitlesRect)
        {
            Destroy(child.gameObject);
        }

        List<TitleUI> titlesList = new List<TitleUI>();
        foreach (KeyValuePair<Title, bool> title in PlayerData.TitleDictionary)
        {
            TitleUI titleUI = Instantiate(TitleUIPrefab).GetComponent<TitleUI>();
            titleUI.Init(title.Key, title.Value, SwitchTitle);

            if (title.Value)
                titleUI.SortOrderRef = 1;
            else
                titleUI.SortOrderRef = -1;

            // If Is Currently Equipped
            if (PlayerData.CurrentTitleID == title.Key.TitleID)
            {
                currSelected = titleUI;
                currSelected.ToggleEquippedOverlay(true);

                ProfileDisplayTitle.sprite = title.Key.LinkedTitle.TitleSprite;
            }
            titlesList.Add(titleUI);
        }
        titlesList.Sort((a, b) => { return b.SortOrderRef.CompareTo(a.SortOrderRef); });
        foreach (TitleUI ui in titlesList)
        {
            ui.gameObject.transform.SetParent(TitlesRect);
            ui.gameObject.transform.localScale = Vector3.one;
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
        string url = ServerDataManager.URL_updateCurrentTitle;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iCurrentTitleID", currSelected.LinkedTitle.TitleID); //Change later btw
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
