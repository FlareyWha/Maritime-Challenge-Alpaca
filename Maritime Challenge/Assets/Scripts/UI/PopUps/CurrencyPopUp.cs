using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPopUp : PopUp
{
    [SerializeField]
    private Text signText, amountText;

    private float lifetime = 1.0f;
    private const float rise_speed = 1.0f;

    private Color32 increaseColor = Color.white;
    private Color32 decreaseColor = Color.white;

    public void Init(int delta_amt)
    {
        amountText.text = Mathf.Abs(delta_amt).ToString();
        if (delta_amt > 0)
        {
            signText.text = "+";
            signText.color = increaseColor;
        }
        else
        {
            signText.text = "-";
            signText.color = decreaseColor;
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
