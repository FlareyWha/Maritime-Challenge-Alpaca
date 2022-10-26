using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CosmeticShopManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] ShopItemPanels;
    [SerializeField]
    private GameObject[] ShopSelectedTabs;
    [SerializeField]
    private Transform HairItemsRect, HeadwearItemsRect, BodyItemsRect, TopsItemRect, BottomsItemRect, ShoeItemRect;

    [SerializeField]
    private int CurrentlySelectedindex = 0;




    public void SetSelectedTab(int index)
    {
        if (CurrentlySelectedindex == index)
            return;

        ShopItemPanels[CurrentlySelectedindex].SetActive(false);
        ShopSelectedTabs[CurrentlySelectedindex].SetActive(false);

        CurrentlySelectedindex = index;

        ShopItemPanels[CurrentlySelectedindex].SetActive(true);
        ShopSelectedTabs[CurrentlySelectedindex].SetActive(true);
    }
}
