using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class AvatarDisplay : MonoBehaviour
{
    [SerializeField]
    private Image[] AvatarDisplayParts;
    [SerializeField]
    private Sprite BlankSprite;

    [SerializeField]
    private Image UnknownAvatar;

    private PlayerAvatarManager playerAvatar = null;

    public void SetPlayer(Player player)
    {
        PlayerAvatarManager playerAvatarManager = player.gameObject.GetComponent<PlayerAvatarManager>();
        if (this.playerAvatar != null)
            this.playerAvatar.OnAvatarChanged -= SetAvatarSprite;

        this.playerAvatar = playerAvatarManager;
        playerAvatar.OnAvatarChanged += SetAvatarSprite;
        InitPlayerAvatarDisplay();

    }

    private void InitPlayerAvatarDisplay()
    {
        UnknownAvatar.gameObject.SetActive(false);
        for (COSMETIC_TYPE i = 0; i < COSMETIC_TYPE.NUM_TOTAL; i++)
        {
            SetAvatarSprite(i, playerAvatar.GetEquippedCosmeticID(i));
        }
    }


    public void SetPlayer(int playerID)
    {
        UnknownAvatar.gameObject.SetActive(false);
        for (COSMETIC_TYPE i = 0; i < COSMETIC_TYPE.NUM_TOTAL; i++)
        {
            SetAvatarSprite(i, PlayerAvatarManager.NullRefNum);
        }


        if (!PlayerData.OthersEquippedCosmeticList.ContainsKey(playerID) || PlayerData.OthersEquippedCosmeticList[playerID].Count == 0)
        {
            UIManager.Instance.WaitAvatarDisplayInit(this, playerID);
            return;
        }

        foreach (Cosmetic cos in PlayerData.OthersEquippedCosmeticList[playerID])
        {
            SetAvatarSprite(cos.CosmeticBodyPartType, cos.CosmeticID);
        }
    }


    public void SetDefault()
    {
        UnknownAvatar.gameObject.SetActive(false);
        for (COSMETIC_TYPE i = 0; i < COSMETIC_TYPE.NUM_TOTAL; i++)
        {
            SetAvatarSprite(i, PlayerAvatarManager.NullRefNum);
        }
    
        SetAvatarSprite(COSMETIC_TYPE.HAIR, 3);
        SetAvatarSprite(COSMETIC_TYPE.TOP, 10);
        SetAvatarSprite(COSMETIC_TYPE.BOTTOM, 11);
        SetAvatarSprite(COSMETIC_TYPE.SHOE, 5);
        SetAvatarSprite(COSMETIC_TYPE.BODY, 2);
        
    }

   

    public void SetUnknown()
    {
        UnknownAvatar.gameObject.SetActive(true);
        for (BODY_PART_TYPE i = 0; i < BODY_PART_TYPE.NUM_TOTAL; i++)
        {
            AvatarDisplayParts[(int)i].sprite = BlankSprite;
        }
    }

    
    public void SetAvatarSprite(COSMETIC_TYPE type, int cosmeticID)
    {
        switch (type)
        {
            case COSMETIC_TYPE.HAIR:
                UpdatePartSprite(BODY_PART_TYPE.HAIR_BACK, cosmeticID);
                UpdatePartSprite(BODY_PART_TYPE.HAIR_FRONT, cosmeticID);
                break;
            default:
                UpdatePartSprite((BODY_PART_TYPE)(int)type, cosmeticID);
                break;
        }

    }

    private void UpdatePartSprite(BODY_PART_TYPE type, int cosmeticID)
    {
        if (cosmeticID == PlayerAvatarManager.NullRefNum)
        {
            AvatarDisplayParts[(int)type].sprite = BlankSprite;
            return;
        }    

        Sprite newSprite = CosmeticManager.Instance.GetDisplaySprite(cosmeticID, type);
        if (newSprite == null) // safe checking
            AvatarDisplayParts[(int)type].sprite = BlankSprite;
        else
            AvatarDisplayParts[(int)type].sprite = newSprite;

    }
}
