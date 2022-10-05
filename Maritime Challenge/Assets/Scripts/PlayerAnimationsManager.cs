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

    private void UpdateAvatarAnimations()
    {
        Debug.Log("Avatar Animations Updated");
        for (int i = 0; i < (int)BodyPartType.NUM_TOTAL; i++)
        {
            animatorHandlers[i].SetAnimations(GetAvatarPart((BodyPartType)i));
        }
    }

    private void UpdateSpecificAnimations(BodyPartType type, int cosmeticID)
    {
        animatorHandlers[(int)type].SetAnimations(type, cosmeticID);
    }


    private void SetAvatar(Player player)
    {
        Debug.Log("Assigned to Event");
        player.OnAvatarChanged += UpdateSpecificAnimations;
        playerAvatar = player.PlayerAvatar;

    }

    private AvatarPart GetAvatarPart(BodyPartType type)
    {
        switch (type)
        {
            case BodyPartType.HAIR_BACK:
            case BodyPartType.HAIR_FRONT:
                return playerAvatar.avatarParts[(int)CosmeticType.HAIR];
            default:
                return playerAvatar.avatarParts[(int)type];
        }
    }
    
}
