
using UnityEngine;

[CreateAssetMenu(fileName = "New Redeemable Item", menuName = "Redeem Item")]
public class RedeemableItemSO : ScriptableObject
{
    public int ID;
    public string Name;
    public int RollarsCost;
    public Sprite VoucherSprite;
}
