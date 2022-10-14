using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationsManager : MonoBehaviour
{
    [SerializeField]
    private AnimatorHandler[] animatorHandlers;

    private PlayerAvatarManager playerAvatar = null;

    private void Start()
    {
        Debug.Log("PlayerAnimsManager Start Called");

        StartCoroutine(Inits());
    }

    IEnumerator Inits()
    {
        playerAvatar = GetComponent<Player>().gameObject.GetComponent<PlayerAvatarManager>();
        playerAvatar.OnAvatarChanged += UpdateSpecificAnimations;

        while (!playerAvatar.IsInitted())
            yield return null;
        InitAvatarAnimations();
    }

    public void InitAvatarAnimations() 
    {
        Debug.Log("Avatar Animations Updated");
        for (BODY_PART_TYPE i = 0; i < BODY_PART_TYPE.NUM_TOTAL; i++)
        {
            animatorHandlers[(int)i].SetAnimations(GetPartID(i));
        }
    }

    public void UpdateSpecificAnimations(COSMETIC_TYPE type, int cosmeticID)
    {
        animatorHandlers[(int)type].SetAnimations(cosmeticID);
    }


   
    private int GetPartID(BODY_PART_TYPE type)  
    {
        switch (type)
        {
            case BODY_PART_TYPE.HAIR_BACK:
            case BODY_PART_TYPE.HAIR_FRONT:
                return playerAvatar.GetEquippedCosmeticID(COSMETIC_TYPE.HAIR);
            default:
                return playerAvatar.GetEquippedCosmeticID((COSMETIC_TYPE)(int)type);
        }
    }

}
