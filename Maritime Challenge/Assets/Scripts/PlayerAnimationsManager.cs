using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsManager : MonoBehaviour
{
    [SerializeField]
    private AnimatorHandler[] animatorHandlers;
    [SerializeField]
    private AvatarSO myAvatar;




    private void Start()
    {
        AvatarCustomisationManager.OnAvatarUpdated += UpdateAvatarAnimations;
        UpdateAvatarAnimations();
    }

    private void UpdateAvatarAnimations()
    {
        for (int i = 0; i < (int)CosmeticType.NUM_TOTAL; i++)
        {
            animatorHandlers[i].SetAnimations(myAvatar.avatarParts[i]);
        }
    }
}
