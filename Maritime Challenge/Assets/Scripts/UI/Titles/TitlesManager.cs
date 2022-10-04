using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    void Awake()
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
            titleUI.Init(title.Key, title.Value);

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

        // set title to newTitle.LinkedTitle in db

        ProfileDisplayTitle.sprite = newTitle.LinkedTitle.LinkedTitle.TitleSprite;
    }

 
}
