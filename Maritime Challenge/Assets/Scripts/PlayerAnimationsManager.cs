using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsManager : MonoBehaviour
{
    [SerializeField]
    private AnimatorHandler hairAnimatorHandler, bodyAnimatorHandler, topAnimatorHandler, bottomAnimatorHandler, shoeAnimatorHandler;
    [SerializeField]
    private AvatarSO myAvatar;


    private void Start()
    {
        UpdateAvatarAnimations();
    }

    private void UpdateAvatarAnimations()
    {
        hairAnimatorHandler.SetAnimations(myAvatar.avatarHair);
        bodyAnimatorHandler.SetAnimations(myAvatar.avatarBody);
        topAnimatorHandler.SetAnimations(myAvatar.avatarTop);
        bottomAnimatorHandler.SetAnimations(myAvatar.avatarBottom);
        shoeAnimatorHandler.SetAnimations(myAvatar.avatarShoe);
    }
}
