using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmetic
{
    public int CosmeticID;
    public int CosmeticBodyPartID;
    public string CosmeticName;
    public int CosmeticPrice;
    public COSMETIC_RARITY CosmeticRarity;
    public COSMETIC_TYPE CosmeticBodyPartType;

    public AvatarCosmetic LinkedCosmetic;

    public Cosmetic(int cosmeticID, string name, int price, COSMETIC_RARITY rarity, COSMETIC_TYPE bodyPartType)
    {
        CosmeticID = cosmeticID;
        CosmeticName = name;
        CosmeticPrice = price;
        CosmeticRarity = rarity;
        CosmeticBodyPartType = bodyPartType;
    }
}

public enum COSMETIC_RARITY
{ 
    COMMON,
    RARE,
    EPIC,
    LEGENDARY,
    NO_COSMETIC_RARITY
}

public enum COSMETIC_TYPE
{
    HAIR = 0,
    HEADWEAR = 1,
    BODY = 2,
    TOP = 3,
    BOTTOM = 4,
    SHOE = 5,

    NUM_TOTAL = 6
}
