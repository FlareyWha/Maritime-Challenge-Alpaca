using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private List<TitleSO> titlesList;

    void Awake()
    {
        foreach (KeyValuePair<Title, bool> title in PlayerData.TitleDictionary)
        {
            title.Key.LinkedTitle = FindCosmeticByID(title.Key.TitleID);
        }
    }

    private TitleSO FindCosmeticByID(int id)
    {
        foreach (TitleSO title in titlesList)
        {
            if (title.ID == id)
                return title;
        }
        Debug.LogWarning("Could not find Title of ID " + id + "!");
        return null;
    }

}
