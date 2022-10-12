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
  
    private Player player = null;

    public void SetPlayer(Player player)
    {
        if (player == null)
        {
            SetUnknown();
            return;
        }
        else
        {
            SetAvatar(player.PlayerAvatar);
            player.OnAvatarChanged += OnPlayerAvatarUpdated;
        }
        if (this.player != null)
            this.player.OnAvatarChanged -= OnPlayerAvatarUpdated;

        this.player = player;

    }


    void SetUnknown()
    {
        UnknownAvatar.gameObject.SetActive(true);
        for (BODY_PART_TYPE i = 0; i < BODY_PART_TYPE.NUM_TOTAL; i++)
        {
            AvatarDisplayParts[(int)i].sprite = BlankSprite;
        }
    }


    private void SetAvatar(AvatarSO avatar)
    {
        UnknownAvatar.gameObject.SetActive(false);
        for (BODY_PART_TYPE i = 0; i < BODY_PART_TYPE.NUM_TOTAL; i++)
        {
            switch (i)
            {
                case BODY_PART_TYPE.HAIR_BACK:
                case BODY_PART_TYPE.HAIR_FRONT:
                    if (avatar.avatarParts[(int)COSMETIC_TYPE.HAIR].cosmetic!= null)
                        UpdatePartSprite(i, avatar.avatarParts[(int)COSMETIC_TYPE.HAIR].cosmetic.ID);
                    break;
                default:
                    if (avatar.avatarParts[(int)i].cosmetic != null)
                        UpdatePartSprite(i, avatar.avatarParts[(int)i].cosmetic.ID);
                    break;
            }
        }


    }

    private void OnPlayerAvatarUpdated(BODY_PART_TYPE type, int cosmeticID)
    {
        UpdatePartSprite(type, cosmeticID);
    }

    private void UpdatePartSprite(BODY_PART_TYPE type, int cosmeticID)
    {
        Sprite newSprite = CosmeticManager.Instance.GetDisplaySprite(cosmeticID, type);
        if (newSprite == null)
            AvatarDisplayParts[(int)type].sprite = BlankSprite;
        else
            AvatarDisplayParts[(int)type].sprite = newSprite;

    }
}
