using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerOrderInLayerTrigger : NetworkBehaviour
{
    [SerializeField]
    private SpriteRenderer[] PlayerSprites;
    private int[] defaultSortingOrder = new int[(int)BODY_PART_TYPE.NUM_TOTAL];


    private List<OnTriggerSetOrder> triggerList = new List<OnTriggerSetOrder>();

    private void Awake()
    {
        for (int i = 0; i < PlayerSprites.Length; i++)
        {
            defaultSortingOrder[i] = PlayerSprites[i].sortingOrder;
        }
    }
    private void Start()
    {
        if (isLocalPlayer)
        {
            for (int i = 0; i < defaultSortingOrder.Length; i++)
            {
                defaultSortingOrder[i] += 3;
            }
        }
    }
    public void SetOrderInLayer(int what)
    {
        for (int i = 0; i < PlayerSprites.Length; i++)
        {
            PlayerSprites[i].sortingOrder = defaultSortingOrder[i] + what;
        }
    }

    public void AddTrigger(OnTriggerSetOrder trigger)
    {
        triggerList.Add(trigger);
        SetOrderInLayer(trigger.OrderInLayer);
    }

    public void RemoveTrigger(OnTriggerSetOrder trigger)
    {
        triggerList.Remove(trigger);
        if (triggerList.Count > 0)
            SetOrderInLayer(triggerList[triggerList.Count - 1].OrderInLayer);
    }

}
