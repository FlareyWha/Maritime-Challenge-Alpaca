using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpManager : MonoBehaviourSingleton<PopUpManager>
{

    [SerializeField]
    private GameObject HPPopUpPrefab;


    private List<PopUp> popUpList = new List<PopUp>();

    public void AddHPChangeText(int deltaHP, Transform entityTransform)
    {
        Debug.Log("Instantiating HP Changed POP UP");
        GameObject popup_go = Instantiate(HPPopUpPrefab, Vector3.zero, Quaternion.identity);
        popup_go.transform.SetParent(entityTransform.Find("Canvas"));
        popup_go.transform.localPosition = Vector3.zero;

        HPPopUp hpPopUp = popup_go.GetComponent<HPPopUp>();
        hpPopUp.Init(deltaHP, entityTransform.gameObject == PlayerData.MyPlayer.gameObject);
        popUpList.Add(hpPopUp);
    }

    private void Update()
    {
        for (int i = 0; i < popUpList.Count; i++)
        {
            if (!popUpList[i].IsActive())
            {
                Destroy(popUpList[i].gameObject);
                popUpList[i] = null;
            }
        }

        popUpList.RemoveAll(s => s == null);
    }
}


