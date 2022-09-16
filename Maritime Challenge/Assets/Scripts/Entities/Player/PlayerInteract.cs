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

    public static void OnEnterInteractable(Interactable interactable)
    {
        if (InRangeList.Count == 0)
        {
            // Enable Interact Button if no others
            UIManager.Instance.EnableInteractButton(interactable.GetInteractMessage());
        }

        InRangeList.Add(interactable);
    }

    public static void OnLeaveInteractable(Interactable interactable)
    {
        InRangeList.Remove(interactable);
        if (InRangeList.Count == 0)
        {
            // Disable Interact Button if no others
            UIManager.Instance.DisableInteractButton();
        }
    }

    public static void ClearInteractables()
    {
        InRangeList.Clear();
        UIManager.Instance.DisableInteractButton();
    }


}
