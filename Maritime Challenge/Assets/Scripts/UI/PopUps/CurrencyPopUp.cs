using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPopUp : PopUp
{
    [SerializeField]
    private Text signText, amountText;
    [SerializeField]
    private Image CurrencyIcon;
    [SerializeField]
    private Sprite TokenIconSprite, RollarIconSprite, HeartIconSprite, XPIconSprite;

    private float lifetime = 1.0f;
    private const float rise_speed = 15.0f;

    private Color32 increaseColor = Color.white;
    private Color32 decreaseColor = new Color32(255, 135, 135, 255);

    public void Init(CURRENCY_TYPE type, int delta_amt)
    {
        amountText.text = Mathf.Abs(delta_amt).ToString();
        if (delta_amt > 0)
        {
            signText.text = "+";
            signText.color = increaseColor;
            amountText.color = increaseColor;
        }
        else
        {
            signText.text = "-";
            signText.color = decreaseColor;
            amountText.color = decreaseColor;
        }


        switch (type)
        {
            case CURRENCY_TYPE.TOKEN:
                CurrencyIcon.sprite = TokenIconSprite;
                break;
            case CURRENCY_TYPE.ROLLAR:
                CurrencyIcon.sprite = RollarIconSprite;
                break; 
            case CURRENCY_TYPE.FRIENDSHIP_XP:
                CurrencyIcon.sprite = HeartIconSprite;
                break;   
            case CURRENCY_TYPE.PLAYER_XP:
                CurrencyIcon.sprite = XPIconSprite;
                break;
            default:
                break;
        }

    }

    public override void Update()
    {
        if (!active)
            return;


        transform.position = new Vector3(transform.position.x,
            transform.position.y + rise_speed * Time.deltaTime, transform.position.z);

        lifetime -= Time.deltaTime;
        if (lifetime < 0.0f)
            active = false;
    }
}

public enum CURRENCY_TYPE
{
    TOKEN,
    ROLLAR,
    FRIENDSHIP_XP,
    PLAYER_XP,

    NUM_TOTAL
}
