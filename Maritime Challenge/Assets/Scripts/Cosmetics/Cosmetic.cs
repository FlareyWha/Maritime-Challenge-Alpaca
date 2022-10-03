using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cosmetic
{
    public string cosmeticName;
    public int cosmeticPrice;
    public CosmeticRarity cosmeticRarity;
    public AvatarBodyPartType cosmeticBodyPartType;

    public Cosmetic(string name, int price, CosmeticRarity rarity, AvatarBodyPartType bodyPartType)
    {
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

