using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CafeManager : MonoBehaviourSingleton<CafeManager>
{
    public bool HasDrink;

    [SerializeField]
    private List<Sit> SitsList;

    public int GetSitID(Sit sit)
    {
        for (int i = 0; i < SitsList.Count; i++)
        {
            if (SitsList[i] == sit)
                return i;
        }
        Debug.LogWarning("Could not find seat!");
        return -1;
    }

    public Sit FindSit(int id)
    {
        foreach (Sit sit in SitsList)
        {
            if (sit.SitID == id)
                return sit;
        }
        Debug.LogWarning("Could not find seat!");
        return null;
    }
}
