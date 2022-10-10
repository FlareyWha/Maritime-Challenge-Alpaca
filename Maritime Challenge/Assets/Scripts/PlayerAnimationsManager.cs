using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsManager : MonoBehaviour
{
    [SerializeField]
    private AnimatorHandler[] animatorHandlers;
    [SerializeField]
    private Player player;

    private AvatarSO playerAvatar;


    private void Start()
    {
        if (player != null)
            SetAvatar(player);
        UpdateAvatarAnimations();
    }

    private void UpdateAvatarAnimations() // Uses Avatar, only works locally:") for local players
    {
        Debug.Log("Avatar Animations Updated");
        for (int i = 0; i < (int)BODY_PART_TYPE.NUM_TOTAL; i++)
        {
            animatorHandlers[i].SetAnimations(GetAvatarPart((BODY_PART_TYPE)i));
        }
    }

    private void UpdateSpecificAnimations(BODY_PART_TYPE type, int cosmeticID)
    {
        animatorHandlers[(int)type].SetAnimations(type, cosmeticID);
    }


    private void SetAvatar(Player player)
    {
        Debug.Log("Assigned to Event");
        player.OnAvatarChanged += UpdateSpecificAnimations;
        playerAvatar = player.PlayerAvatar;

    }

    private AvatarPart GetAvatarPart(BODY_PART_TYPE type)  // Uses Avatar, only works locally:") for local players
    {
        switch (type)
        {
            case BODY_PART_TYPE.HAIR_BACK:
            case BODY_PART_TYPE.HAIR_FRONT:
                return playerAvatar.avatarParts[(int)COSMETIC_TYPE.HAIR];
            default:
                return playerAvatar.avatarParts[(int)type];
        }
    }
    
}
