using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmetic
{
    public int CosmeticID;
    public int CosmeticBodyPartID;
    public string CosmeticName;
    public int CosmeticPrice;
    public CosmeticRarity CosmeticRarity;
    public CosmeticType CosmeticBodyPartType;

    public AvatarCosmetic LinkedCosmetic;

    public Cosmetic(int cosmeticID, string name, int price, CosmeticRarity rarity, CosmeticType bodyPartType)
    {
        CosmeticID = cosmeticID;
        CosmeticName = name;
        CosmeticPrice = price;
        CosmeticRarity = rarity;
        CosmeticBodyPartType = bodyPartType;
    }
}

public enum CosmeticRarity
{ 
    COMMON,
    RARE,
    EPIC,
    LEGENDARY,
    NO_COSMETIC_RARITY
}

