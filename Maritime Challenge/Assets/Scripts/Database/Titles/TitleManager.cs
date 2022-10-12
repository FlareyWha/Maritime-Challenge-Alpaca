using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleManager : MonoBehaviourSingleton<TitleManager>
{
    [SerializeField]
    private List<TitleSO> titlesList;

    protected override void Awake()
    {
        base.Awake();

        foreach (KeyValuePair<Title, bool> title in PlayerData.TitleDictionary)
        {
            title.Key.LinkedTitle = FindTitleByID(title.Key.TitleID);
        }
    }

    public TitleSO FindTitleByID(int id)
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
