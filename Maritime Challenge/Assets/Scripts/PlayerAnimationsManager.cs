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
        for (int i = 0; i < (int)CosmeticType.NUM_TOTAL; i++)
        {
            animatorHandlers[i].SetAnimations(myAvatar.avatarParts[i]);
        }
    }

    private void SetAvatar(Player player)
    {
        if (player == PlayerData.MyPlayer)
            AvatarCustomisationManager.OnAvatarUpdated += UpdateAvatarAnimations;

        myAvatar = player.PlayerAvatar;

    }

    
}
