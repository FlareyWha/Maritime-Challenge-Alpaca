using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPopUp : PopUp
{
    [SerializeField]
    private Text signText, amountText;

    private float lifetime = 1.0f;
    private const float rise_speed = 10.0f;

    private Color32 increaseColor = Color.gray;
    private Color32 decreaseColor = Color.gray;

    public void Init(int delta_amt)
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

    }

    public override void Update()
    {
        if (!active)
            return;

        Debug.Log("Currency PopUp Updating");

        transform.position = new Vector3(transform.position.x,
            transform.position.y + rise_speed * Time.deltaTime, transform.position.z);

        lifetime -= Time.deltaTime;
        if (lifetime < 0.0f)
            active = false;
    }
}
