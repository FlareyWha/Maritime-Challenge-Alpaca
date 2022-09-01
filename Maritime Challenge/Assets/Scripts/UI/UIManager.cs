using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    // Main/Hub
    public PlayerFollowCamera Camera;
    public Joystick Joystick;

    [SerializeField]
    private Image MenuPanelMask;
    [SerializeField]
    private Image ChatPanelMask;

    // Profile Page : TBC -> Might Shift to ProfileManager
    [SerializeField]
    private Image NamecardMask, NamecardImage;
    [SerializeField]
    private Sprite ShortNamecardSprite, LongNamecardSprite;
    [SerializeField]
    private GameObject ProfilePageButtons;

    // Interaction UIs
    [SerializeField]
    private ProfileNamecard InteractNamecard;


    private const float OPEN_MENU_ANIM_TIME = 0.5f;
    private const float CLOSE_MENU_ANIM_TIME = 0.5f;
    //private float timer = 0.0f;


    public void ToggleMenu(Button button)
    {
        if (MenuPanelMask.fillAmount == 0.0f)
            StartCoroutine(ToggleSlideAnim(MenuPanelMask, true, OPEN_MENU_ANIM_TIME, button));
        else
            StartCoroutine(ToggleSlideAnim(MenuPanelMask, false, CLOSE_MENU_ANIM_TIME, button));
    }

    public void ToggleChat(Button button)
    {
        if (ChatPanelMask.fillAmount == 0.0f)
            StartCoroutine(ToggleSlideAnim(ChatPanelMask, true, OPEN_MENU_ANIM_TIME, button));
        else
            StartCoroutine(ToggleSlideAnim(ChatPanelMask, false, CLOSE_MENU_ANIM_TIME, button));
    }

    public void ToggleNamecard(Button arrowButton)
    {
        if (arrowButton.transform.rotation.eulerAngles.z == 0)
            StartCoroutine(ExtendNamecardAnim(1.0f, arrowButton));
        else
            StartCoroutine(RetractNamecardAnim(1.0f, arrowButton));
    }

    public void SetInteractNamecardDetails(Player player) // TBC dk get here or frm outside class
    {
        if (FriendsManager.CheckIfFriends(player.GetUID()))
            InteractNamecard.SetDetails(player);
        else if (ContactsManager.CheckIfKnown(player.GetUID()))
            InteractNamecard.SetHidden(player.GetUID());
        else
            InteractNamecard.SetUnknown(player.GetUID());
    }

    public void AddInteractedAsFriend()
    {
        int id = InteractNamecard.GetPlayerID();
        FriendsManager.Instance.AddFriend(id, PlayerData.FindPlayerNameByID(id));
        InteractNamecard.SetDetails(PlayerInteract.interactPlayer);
    }
    
    public void ShowInteractNamecard()
    {
        StartCoroutine(ToggleFlyInAnim(InteractNamecard.gameObject, new Vector3(0, -900, 0), Vector3.zero, 1.0f, null));
    }

    public void ShowInteractNamecard(Button button)
    {
        StartCoroutine(ToggleFlyInAnim(InteractNamecard.gameObject, new Vector3(0, -900, 0), Vector3.zero, 1.0f, button));
    }

    public void HideInteractNamecard(Button button)
    {
        StartCoroutine(ToggleFlyOutAnim(InteractNamecard.gameObject, Vector3.zero, new Vector3(0, -900, 0), 1.0f, button));
        PlayerInteract.interactPlayer = null;
    }

    static public IEnumerator ToggleSlideAnim(Image mask, bool open, float anim_time, Button button)
    {
        if (button != null)
            button.interactable = false;
        if (open)
            mask.gameObject.SetActive(true);

        float fade_rate = 1.0f / anim_time;
        if (!open)
            fade_rate *= -1;

        float timer = anim_time;
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

        if (button != null)
            button.interactable = true;
        if (!open)
            mask.gameObject.SetActive(false);
    }

    static public IEnumerator ToggleFlyInAnim(GameObject uiGO, Vector3 startPos, Vector3 targetPos, float anim_time, Button button)
    {
        if (button != null)
            button.interactable = false;

        uiGO.gameObject.SetActive(true);

        uiGO.transform.localPosition = startPos;
        Vector3 fly_rate = (targetPos - startPos) / anim_time;
       
        float timer = anim_time;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            uiGO.transform.localPosition += fly_rate * Time.deltaTime;
            yield return null;
        }

        if (button != null)
            button.interactable = true;
     
    }

    static public IEnumerator ToggleFlyOutAnim(GameObject uiGO, Vector3 startPos, Vector3 targetPos, float anim_time, Button button)
    {
        if (button != null)
            button.interactable = false;

        uiGO.transform.localPosition = startPos;
        Vector3 fly_rate = (targetPos - startPos) / anim_time;

        float timer = anim_time;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            uiGO.transform.localPosition += fly_rate * Time.deltaTime;
            yield return null;
        }

        if (button != null)
            button.interactable = true;

        uiGO.gameObject.SetActive(false);
    }


    private IEnumerator ExtendNamecardAnim(float anim_time, Button button)
    {
        // Hide Arrow Button
        button.gameObject.SetActive(false);
        // Hide Other Buttons
        ProfilePageButtons.SetActive(false);

        // Extend Namecard
        NamecardMask.fillAmount = 0.42f;
        NamecardImage.sprite = LongNamecardSprite;
        RectTransform namecardRect = NamecardImage.GetComponent(typeof(RectTransform)) as RectTransform;
        namecardRect.sizeDelta = new Vector2(691, 1008);
        NamecardImage.transform.localPosition =
            new Vector3(NamecardImage.transform.localPosition.x, 0, NamecardImage.transform.localPosition.z);

        float timer = anim_time;
        float extend_mask_rate = (1.0f - 0.42f) / anim_time;
        
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            NamecardMask.fillAmount += extend_mask_rate * Time.deltaTime;
          
            yield return null;
        }
        NamecardMask.fillAmount = 1.0f;
       

        // Rotate and Show Arrow Button
        button.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180));
        button.gameObject.SetActive(true);
    }

    private IEnumerator RetractNamecardAnim(float anim_time, Button button)
    {
        // Hide Arrow Button
        button.gameObject.SetActive(false);

        NamecardMask.fillAmount = 1.0f;
        float timer = anim_time;
        float extend_mask_rate = (1.0f - 0.42f) / anim_time;

        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            NamecardMask.fillAmount -= extend_mask_rate * Time.deltaTime;

            yield return null;
        }
        NamecardMask.fillAmount = 0.5f;
        // Retract Namecard
        NamecardImage.sprite = ShortNamecardSprite;
        RectTransform namecardRect = NamecardImage.GetComponent(typeof(RectTransform)) as RectTransform;
        namecardRect.sizeDelta = new Vector2(682, 487);
        NamecardImage.transform.localPosition =
            new Vector3(NamecardImage.transform.localPosition.x, 260.5f, NamecardImage.transform.localPosition.z);


        // Rotate and Show Arrow Button
        button.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        button.gameObject.SetActive(true);
        // Show Profile Buttons
        ProfilePageButtons.SetActive(true);
    }

    public bool IsInteractButtonClicked()
    {
        return false;
    }


    public void ToggleJoystick(bool on)
    {
        Joystick.gameObject.SetActive(on);
        Joystick.ResetJoystick();
    }
   
}
