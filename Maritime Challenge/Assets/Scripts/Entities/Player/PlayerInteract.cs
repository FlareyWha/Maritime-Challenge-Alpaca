using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class PlayerInteract : NetworkBehaviour
{

    public static List<Interactable> InRangeList = new List<Interactable>();

   

    public override void OnStartAuthority()
    {
        UIManager.Instance.InteractButton.onClick.AddListener(OnInteractButtonClicked);
    }

    void Update()
    {
     
    }

    [Client]
    private void OnInteractButtonClicked()
    {
        if (InRangeList.Count > 0)
        {
            InRangeList[0].Interact();
        }
    }


}
