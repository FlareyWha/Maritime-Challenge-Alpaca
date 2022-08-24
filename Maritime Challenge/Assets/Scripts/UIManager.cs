using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    public PlayerFollowCamera Camera;

    public Joystick Joystick;

    public Image MenuPanelMask;


    private const float OPEN_MENU_ANIM_TIME = 0.5f;
    private const float CLOSE_MENU_ANIM_TIME = 0.5f;
    private float timer = 0.0f;



    public void ToggleMenu(Button button)
    {
        if (MenuPanelMask.fillAmount == 0.0f)
            StartCoroutine(OpenMenuAnim(button));
        else
            StartCoroutine(CloseMenuAnim(button));
    }

    IEnumerator OpenMenuAnim(Button button)
    {
        button.interactable = false;

        float fade_rate = 1.0f / OPEN_MENU_ANIM_TIME;

        timer = OPEN_MENU_ANIM_TIME;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            MenuPanelMask.fillAmount += fade_rate * Time.deltaTime;
            yield return null;
        }

        MenuPanelMask.fillAmount = 1.0f;

        button.interactable = true;
    }

    IEnumerator CloseMenuAnim(Button button)
    {
        button.interactable = false;

        float fade_rate = 1.0f / CLOSE_MENU_ANIM_TIME;

        timer = CLOSE_MENU_ANIM_TIME;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            MenuPanelMask.fillAmount -= fade_rate * Time.deltaTime;
            yield return null;
        }

        MenuPanelMask.fillAmount = 0.0f;

        button.interactable = true;
    }
}
