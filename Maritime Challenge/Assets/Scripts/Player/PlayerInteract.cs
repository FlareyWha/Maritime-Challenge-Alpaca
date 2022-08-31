using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Networking;

public class PlayerInteract : NetworkBehaviour
{

    private PlayerUI playerUI = null;
    private Vector2 playerSize;

    private bool isInteractOpen = false;


    void Start()
    {
        playerUI = GetComponent<PlayerUI>();

        // Get Player Sprite Size
        playerSize = GetComponent<SpriteRenderer>().bounds.size;
        float pixelsPerUnit = GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;

        playerSize.x *= pixelsPerUnit;
        playerSize.y *= pixelsPerUnit;

        Debug.Log("PLayer Sixe: " + playerSize);
    }

    void Update()
    {
        if (isLocalPlayer)
            return;

        if (!isInteractOpen && InputManager.InputActions.Main.Tap.WasPressedThisFrame() && IsWithinPlayer())
        {
            isInteractOpen = true;
            playerUI.ShowInteractPanel();
        }
    }

    public void CloseInteractPanel()
    {
        isInteractOpen = false;
        playerUI.HideInteractPanel();
    }

    bool IsWithinPlayer()
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

    public void AddFriend()
    {
        Player player = gameObject.GetComponent<Player>();
        StartCoroutine(StartAddFriend(player.GetUID()));
    }

    IEnumerator StartAddFriend(int otherUID)
    {
        string url = ServerDataManager.URL_addFriend;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("OtherUID", otherUID);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);

                //Add friend to the friend list
                PlayerData.FriendList.Add(PlayerData.UID, otherUID);
                break;
            default:
                Debug.LogError("Friend cannot be added");
                break;
        }
    }
}
