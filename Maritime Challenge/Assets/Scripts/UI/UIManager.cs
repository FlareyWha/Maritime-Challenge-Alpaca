using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    public PlayerFollowCamera Camera;

    public Joystick Joystick;

    public Image MenuPanelMask;
    public Image ChatPanelMask;


    private const float OPEN_MENU_ANIM_TIME = 0.5f;
    private const float CLOSE_MENU_ANIM_TIME = 0.5f;
    private float timer = 0.0f;



    public void ToggleMenu(Button button)
    {
        if (MenuPanelMask.fillAmount == 0.0f)
            StartCoroutine(ToggleSlideAnim(MenuPanelMask, true, button));
        else
            StartCoroutine(ToggleSlideAnim(MenuPanelMask, false, button));
    }



    public void ToggleChat(Button button)
    {
        if (ChatPanelMask.fillAmount == 0.0f)
            StartCoroutine(ToggleSlideAnim(ChatPanelMask, true, button));
        else
            StartCoroutine(ToggleSlideAnim(ChatPanelMask, false, button));
    }

    IEnumerator ToggleSlideAnim(Image mask, bool open, Button button)
    {
        button.interactable = false;
        if (open)
            mask.gameObject.SetActive(true);

        float fade_rate = 1.0f / OPEN_MENU_ANIM_TIME;
        if (!open)
            fade_rate *= -1;

        timer = OPEN_MENU_ANIM_TIME;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            mask.fillAmount += fade_rate * Time.deltaTime;
            yield return null;
        }

        if (open)
            mask.fillAmount = 1.0f;
        else
            mask.fillAmount = 0.0f;


        button.interactable = true;
        if (!open)
            mask.gameObject.SetActive(false);
    }

    public bool IsInteractButtonClicked()
    {
        return false;
    }

   
}
