using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPPopUp : PopUp
{
    [SerializeField]
    private Text amountText;

    private float lifetime = 1.0f;
    private const float rise_speed = 1.0f;

    public void Init(int amt, bool is_negative, bool isPlayer)
    {
        amountText.text = amt.ToString();

        if (isPlayer)
        {
            if (is_negative)
                amountText.color = Color.red;
            else
                amountText.color = Color.green;
        }
        else
        {
            if (is_negative)
                amountText.color = Color.white;
            else
                amountText.color = Color.green;
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
