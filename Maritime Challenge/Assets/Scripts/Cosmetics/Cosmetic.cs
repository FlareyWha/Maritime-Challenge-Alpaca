using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmetic
{
    public int cosmeticID;
    public int cosmeticBodyPartID;
    public string cosmeticName;
    public int cosmeticPrice;
    public CosmeticRarity cosmeticRarity;
    public CosmeticType cosmeticBodyPartType;

    public AvatarCosmetic LinkedCosmetic;

    public Cosmetic(int cosmeticID, string name, int price, CosmeticRarity rarity, CosmeticType bodyPartType)
    {
        this.cosmeticID = cosmeticID;
        cosmeticName = name;
        cosmeticPrice = price;
        cosmeticRarity = rarity;
        cosmeticBodyPartType = bodyPartType;
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

