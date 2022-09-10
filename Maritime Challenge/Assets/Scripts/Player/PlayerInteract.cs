using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
using UnityEngine.Networking;

public class PlayerInteract : NetworkBehaviour
{

    private PlayerUI playerUI = null;
    private Vector2 playerSize;

    private bool isInteractOpen = false;

    public static Player interactPlayer = null;
    public static List<Interactable> InRangeList = new List<Interactable>();

    void Start()
    {
        playerUI = GetComponent<PlayerUI>();

        // Get Player Sprite Size
        playerSize = GetComponent<SpriteRenderer>().bounds.size;
        float pixelsPerUnit = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        playerSize.x *= pixelsPerUnit;
        playerSize.y *= pixelsPerUnit;
    }

    public override void OnStartAuthority()
    {
        UIManager.Instance.InteractButton.onClick.AddListener(OnInteractButtonClicked);
    }

    void Update()
    {
      
        // OTHER CLIENTS INTERACT WITH LOCAL PLAYER FOR VIEW MENU
        if (!isLocalPlayer && !isInteractOpen && InputManager.InputActions.Main.Tap.WasPressedThisFrame() && IsWithinPlayer())
        {
            isInteractOpen = true;
            playerUI.ShowInteractPanel();
        }
    }

    private void OnInteractButtonClicked()
    {
        if (InRangeList.Count > 0)
        {
            InRangeList[0].Interact();
        }
    }

    public void CloseInteractPanel()
    {
        isInteractOpen = false;
        playerUI.HideInteractPanel();
    }

    public void ViewProfile(Button button)
    {
        Player player = gameObject.GetComponent<Player>();

        //Unlock the phonebook data if other isnt unlocked to begin with
        if (!PlayerData.PhonebookData[player.GetUID()].Unlocked)
        {
            StartCoroutine(UpdatePhonebookOtherUnlocked(player));
            PlayerData.MyPlayer.UpdateXPLevels(300);
        }
        else
        {
            UIManager.Instance.SetInteractNamecardDetails(player);
        }

        UIManager.Instance.ShowInteractNamecard(button);
        interactPlayer = player;
    }

    IEnumerator UpdatePhonebookOtherUnlocked(Player player)
    {
        string url = ServerDataManager.URL_updatePhonebookOtherUnlocked;
        Debug.Log(url);

        int otherUID = player.GetUID();

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("OtherUID", otherUID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                Debug.Log(webreq.downloadHandler.text);

                PlayerData.PhonebookData[otherUID].Unlocked = true;

                UIManager.Instance.SetInteractNamecardDetails(player);

                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }
 
    private bool IsWithinPlayer()
    {
        Vector2 touchPos = InputManager.InputActions.Main.TouchPosition.ReadValue<Vector2>();
        Vector3 playerPos = Camera.main.WorldToScreenPoint(transform.position);
        if (touchPos.x < playerPos.x + playerSize.x * 0.5f && touchPos.x > playerPos.x - playerSize.x * 0.5f
            && touchPos.y > playerPos.y - playerSize.y * 0.5f && touchPos.y < playerPos.y + playerSize.y * 0.5f)
        {
            return true;
        }

        //Debug.Log(transform.position);
        return false;
    }
}
