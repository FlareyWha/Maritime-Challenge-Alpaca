using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviourSingleton<UIManager>
{
    // Screen UI
    [SerializeField]
    private int UILayer;

    // Main/Hub
    public PlayerFollowCamera Camera;
    public Joystick Joystick;

    [SerializeField]
    private GameObject MenuGO, ChatGO;
    [SerializeField]
    private Image MenuPanelMask;
    [SerializeField]
    private Image ChatPanelMask;
    [SerializeField]
    private GameObject LoadingScreen;

    public Button InteractButton;
    [SerializeField]
    private Text InteractButtonText;

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
 
    [SerializeField]
    private GuildInfoPanel guildInfoPanel;
    public GuildInfoPanel GuildInfoPanel
    {
        get { return guildInfoPanel; }
        private set { }
    }


    private const float OPEN_MENU_ANIM_TIME = 0.5f;
    private const float CLOSE_MENU_ANIM_TIME = 0.5f;

    //private float timer = 0.0f;

    protected override void Awake()
    {
        base.Awake();

        ContactsManager.OnNewRightShipediaEntry += OnNewRightShipediaEntry;
        FriendsManager.OnFriendListUpdated += OnFriendListUpdated;
        FriendsManager.OnNewFriendDataSaved += OnFriendDataSaved;
        FriendRequestHandler.OnFriendRequestSent += OnFriendRequestsUpdated;
        FriendRequestHandler.OnFriendRequestDeleted += OnFriendRequestsUpdated;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        ContactsManager.OnNewRightShipediaEntry -= OnNewRightShipediaEntry;
        FriendsManager.OnFriendListUpdated -= OnFriendListUpdated;
        FriendsManager.OnNewFriendDataSaved -= OnFriendDataSaved;
        FriendRequestHandler.OnFriendRequestSent -= OnFriendRequestsUpdated;
        FriendRequestHandler.OnFriendRequestDeleted -= OnFriendRequestsUpdated;
    }

    public void ToggleMainUI(bool show)
    {
        ToggleJoystick(show);
        MenuGO.SetActive(show);
        ChatGO.SetActive(show);
        InteractButton.gameObject.SetActive(show);

    }

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
            StartCoroutine(ExtendNamecardAnim(0.6f, arrowButton));
        else
            StartCoroutine(RetractNamecardAnim(0.6f, arrowButton));
    }

    public void EnableInteractButton(string interact_text)
    {
        InteractButton.gameObject.SetActive(true);

        SetInteractButtonMessage(interact_text);
    }

    public void SetInteractButtonMessage(string interact_text)
    {
        InteractButtonText.text = interact_text;
        SetWidthByTextWidth(InteractButton.gameObject, InteractButtonText, 10);
    }

    public void DisableInteractButton()
    {
        InteractButton.gameObject.SetActive(false);
    }
    
    public void ToggleLoadingScreen(bool show)
    {
        LoadingScreen.SetActive(show);
    }

    public void SetInteractNamecardDetails(Player player) // TBC dk get here or frm outside class
    {
        if (FriendsManager.CheckIfFriends(player.GetUID()))
            InteractNamecard.SetDetails(player);
        else if (ContactsManager.CheckIfKnown(player.GetUID()))
            InteractNamecard.SetHidden(player);
        else
            InteractNamecard.SetUnknown(player.GetUID());
    }

    public void SetInteractNamecardDetails(int playerID)
    {
        if (FriendsManager.CheckIfFriends(playerID))
            SetFriendInteractNamecardDetails(playerID);
        else if (ContactsManager.CheckIfKnown(playerID))
            InteractNamecard.SetHidden(PlayerData.FindPlayerInfoByID(playerID));
        else
            InteractNamecard.SetUnknown(playerID);
    }

    public void SetExampleInteractNamecardDetails()
    {
        InteractNamecard.SetExampleDetails();
    }

    private void OnNewRightShipediaEntry(int id)
    {
        SetInteractNamecardDetails(InteractNamecard.GetPlayerID());
    }

    private void SetFriendInteractNamecardDetails(int id)
    {
        foreach (FriendInfo friend in PlayerData.FriendDataList)
        {
            if (friend.UID == id)
            {
                FriendsManager.Instance.GetFriendDataInfo(id);
                InteractNamecard.SetDetails(PlayerData.FindFriendInfoByID(id));
                return;
            }
        }

        FriendsManager.Instance.GetFriendDataInfo(id);
    }


    public void ShowInteractNamecard()
    {
        StartCoroutine(ToggleFlyInAnim(InteractNamecard.gameObject, true, new Vector3(0, -900, 0), Vector3.zero, 1.0f, null));
    }

    public void ShowInteractNamecard(Button button)
    {
        StartCoroutine(ToggleFlyInAnim(InteractNamecard.gameObject, true, new Vector3(0, -900, 0), Vector3.zero, 0.5f, button));
    }

    public void HideInteractNamecard(Button button)
    {
        StartCoroutine(ToggleFlyOutAnim(InteractNamecard.gameObject, true, Vector3.zero, new Vector3(0, -900, 0), 0.3f, button));
        PlayerUI.interactPlayer = null;
    }

    private void OnFriendRequestsUpdated(int sender_id, int rec_id)
    {
        if (InteractNamecard.GetPlayerID() == sender_id || InteractNamecard.GetPlayerID() == rec_id)
            SetInteractNamecardDetails(InteractNamecard.GetPlayerID());
    }
    private void OnFriendDataSaved(FriendInfo friend)
    {
        if (InteractNamecard.GetPlayerID() != friend.UID)
            return;

        InteractNamecard.SetDetails(friend);
    }

    private void OnFriendListUpdated()
    {
        SetInteractNamecardDetails(InteractNamecard.GetPlayerID());
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

    static public IEnumerator ToggleFadeAnim(CanvasGroup canvasGroup, float start_alpha, float end_alpha, float anim_time)
    {
        canvasGroup.alpha = start_alpha;

        float rate = (end_alpha - start_alpha) / anim_time;
        float timer = anim_time;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            canvasGroup.alpha += rate * Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = end_alpha;
     
    }

    static public IEnumerator ToggleFlyInAnim(RectTransform rectTransform, Vector3 startPos, Vector3 targetPos, float anim_time, Button button)
    {
        if (button != null)
            button.interactable = false;

        rectTransform.anchoredPosition = startPos;
        Vector2 fly_rate = (targetPos - startPos) / anim_time;

        float timer = anim_time;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            rectTransform.anchoredPosition += fly_rate * Time.deltaTime;
            yield return null;
        }

        if (button != null)
            button.interactable = true;
    }

    static public IEnumerator ToggleFlyInAnim(GameObject uiGO, bool changeActive, Vector3 startPos, Vector3 targetPos, float anim_time, Button button)
    {
        if (button != null)
            button.interactable = false;

        if (changeActive)
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

    static public IEnumerator ToggleFlyOutAnim(RectTransform rectTransform, Vector3 startPos, Vector3 targetPos, float anim_time, Button button)
    {
        if (button != null)
            button.interactable = false;

        rectTransform.anchoredPosition = startPos;
        Vector2 fly_rate = (targetPos - startPos) / anim_time;

        float timer = anim_time;
        while (timer > 0.0f)
        {
            timer -= Time.deltaTime;
            rectTransform.anchoredPosition += fly_rate * Time.deltaTime;
            yield return null;
        }

        if (button != null)
            button.interactable = true;
    }

    static public IEnumerator ToggleFlyOutAnim(GameObject uiGO, bool changeActive, Vector3 startPos, Vector3 targetPos, float anim_time, Button button)
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

        if (changeActive)
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
    public void ToggleJoystick(bool on)
    {
        Joystick.gameObject.SetActive(on);
        Joystick.ResetJoystick();
    }

    // Returns 'true' if we touched or hovering on Unity UI element.
    public static bool IsPointerOverUIElement()
    {
       
        return IsPointerOverUIElement(GetEventSystemRaycastResults());
    }

    // Returns 'true' if we touched or hovering on Unity UI element.
    private static bool IsPointerOverUIElement(List<RaycastResult> eventSystemRaysastResults)
    {
        for (int index = 0; index < eventSystemRaysastResults.Count; index++)
        {
            RaycastResult curRaysastResult = eventSystemRaysastResults[index];
            if (curRaysastResult.gameObject.layer == Instance.UILayer)
                return true;
        }
        return false;
    }

    // Gets all event system raycast results of current mouse or touch position.
    private static List<RaycastResult> GetEventSystemRaycastResults()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        return raysastResults;
    }

    public static void SetWidthByTextWidth(GameObject toChange, Text toRef, float padding)
    {
        float text_width = toRef.preferredWidth;
        RectTransform rt = toChange.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(text_width + padding * 2, rt.rect.height);
    }

    public static void SetHeightByTextHeight(GameObject toChange, Text toRef)
    {
        float text_height = toRef.preferredHeight;
        RectTransform rt = toChange.GetComponent(typeof(RectTransform)) as RectTransform;
        rt.sizeDelta = new Vector2(rt.rect.width, text_height);
    }

    
}
