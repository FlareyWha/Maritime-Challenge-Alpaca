using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleUnlockPopUp : MonoBehaviour
{
    [SerializeField]
    private Image TitleImage;

    public void InitTitle(Sprite titleSprite)
    {
        TitleImage.sprite = titleSprite;
    }



    public void ClosePopUp()
    {
        Destroy(this.gameObject);
    }
}
