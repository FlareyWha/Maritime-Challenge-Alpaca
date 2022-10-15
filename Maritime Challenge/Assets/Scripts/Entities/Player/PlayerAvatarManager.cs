using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerAvatarManager : NetworkBehaviour
{
    private readonly SyncList<int> EquippedCosmeticIDs = new SyncList<int>();

    private CosmeticSO[] EquippedCosmetics = new CosmeticSO[(int)COSMETIC_TYPE.NUM_TOTAL];

    public static int NullRefNum = -1;

    public delegate void AvatarChanged(COSMETIC_TYPE type, int cosmeticID);
    public event AvatarChanged OnAvatarChanged;

    private void Start()
    {
        EquippedCosmeticIDs.Callback += OnAvatarCosmeticsUpdated;
    }
    public override void OnStartLocalPlayer()
    {
        StartCoroutine(InitCosmetics());
    }

    IEnumerator InitCosmetics()
    {
        while (PlayerData.EquippedCosmeticsList.Count == 0)
            yield return null;

        List<int> equippedList = new List<int>();
        for (int i = 0; i < (int)COSMETIC_TYPE.NUM_TOTAL; i++)
        {
            equippedList.Add(NullRefNum);
        }
        for (int i = 0; i < PlayerData.EquippedCosmeticsList.Count; i++)
        {
            equippedList[(int)PlayerData.EquippedCosmeticsList[i].CosmeticBodyPartType] = PlayerData.EquippedCosmeticsList[i].CosmeticID;
        }
        InitCosmetics(equippedList);
        Debug.Log("PlayerAvatarManager inits Called");
    }

    [Command]
    private void InitCosmetics(List<int> cosmeticsID)
    {
        EquippedCosmeticIDs.Clear();
        foreach (int id in cosmeticsID)
            EquippedCosmeticIDs.Add(id);
    }

    private void OnAvatarCosmeticsUpdated(SyncList<int>.Operation op, int index, int oldItem, int newItem)
    {
        if (newItem == NullRefNum)
        {
            EquippedCosmetics[index] = null;
            return;
        }
        EquippedCosmetics[index] = CosmeticManager.Instance.FindCosmeticByID(newItem);
    }

    public CosmeticSO GetEquippedCosmetic(COSMETIC_TYPE type)
    {
        return EquippedCosmetics[(int)type];
    }

    public int GetEquippedCosmeticID(COSMETIC_TYPE type)
    {
        return EquippedCosmeticIDs[(int)type];
    }


    public void AvatarCosmeticChanged(COSMETIC_TYPE type, int cosmeticID)
    {
        CallAvatarChanged(type, cosmeticID);
    }

    [Command]
    private void CallAvatarChanged(COSMETIC_TYPE type, int cosmeticID)
    {
        EquippedCosmeticIDs[(int)type] = cosmeticID;
        InvokeAvatarChanged(type, cosmeticID);
    }

    [ClientRpc]
    private void InvokeAvatarChanged(COSMETIC_TYPE type, int cosmeticID)
    {
        OnAvatarChanged?.Invoke(type, cosmeticID);
    }

    public bool IsInitted()
    {
        return EquippedCosmeticIDs.Count == (int)COSMETIC_TYPE.NUM_TOTAL && EquippedCosmetics.Length == (int)COSMETIC_TYPE.NUM_TOTAL;
    }
}
