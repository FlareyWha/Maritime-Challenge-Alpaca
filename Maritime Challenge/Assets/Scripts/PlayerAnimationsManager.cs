using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsManager : MonoBehaviour
{
    [SerializeField]
    private AnimatorHandler[] animatorHandlers;
    [SerializeField]
    private Player player;

    private AvatarSO myAvatar;


    private void Start()
    {
        if (player != null)
            SetAvatar(player);
        UpdateAvatarAnimations();
    }

    private void UpdateAvatarAnimations()
    {
        Debug.Log("Avatar Animations Updated");
        for (int i = 0; i < (int)CosmeticType.NUM_TOTAL; i++)
        {
            animatorHandlers[i].SetAnimations(myAvatar.avatarParts[i]);
        }
    }

    private void UpdateSpecificAnimations(CosmeticType type, int cosmeticID, string partTypeName)
    {
        animatorHandlers[(int)type].SetAnimations(cosmeticID, partTypeName);
    }


    private void SetAvatar(Player player)
    {
        Debug.Log("Assigned to Event");
        player.OnAvatarChanged += UpdateSpecificAnimations;
        myAvatar = player.PlayerAvatar;

    }

    
}
