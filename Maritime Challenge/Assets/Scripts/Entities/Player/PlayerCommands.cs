using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;


// Client - Function runs only on Clients
// ClientRpc - Function Called by Server, Runs it on the same GO on all Clients
// Command - When Function is Called, It is Run on the Server

public class PlayerCommands : NetworkBehaviour
{
    public override void OnStartAuthority()
    {
        PlayerData.CommandsHandler = this;
    }

    public void SwitchSubScene(string sceneName, Vector2 spawnPos)
    {
        PlayerInteract.ClearInteractables();
        RequestSwitchToSubScene(PlayerData.activeSubScene, sceneName, spawnPos);

        //StartCoroutine(WaitEnterScene(sceneName));
    }

    [Command]
    private void RequestSwitchToSubScene(string currSceneName, string sceneName, Vector2 spawnPos)
    {
        SceneManager.Instance.EnterNetworkedSubScene(netIdentity, currSceneName, sceneName, spawnPos);
    }


    [Command]
    public void SendPlayerSeatedEvent(int sitID, Player player)
    {
        SetPlayerSeated(sitID, player);
    }

    [ClientRpc]
    public void SetPlayerSeated(int sitID, Player player)
    {
        Sit.Sits[sitID].PlayerSeated = player;
        Sit.Sits[sitID].UpdateInteractMessage();
    }

   
    public void SendAvatarChanged(BodyPartType type, int cosmeticID)
    {
        Debug.Log("SendAvatarChanged()");
        CallAvatarChanged(type, cosmeticID);
    }

    [Command]
    private void CallAvatarChanged(BodyPartType type, int cosmeticID)
    {
        Debug.Log("Command Received: Call Avatar Changed()");
        InvokeAvatarChanged(type, cosmeticID);
    }

    [ClientRpc]
    private void InvokeAvatarChanged(BodyPartType type, int cosmeticID)
    {
        Debug.Log("ClientRpc Received, Invoke Avatar Changed");
        GetComponent<Player>().InvokeAvatarChangedEvent(type, cosmeticID);
    }

    IEnumerator WaitEnterScene(string sceneName)
    {
        Debug.Log("�nabling Loading Screen");
        UIManager.Instance.ToggleLoadingScreen(true);

        while (PlayerData.activeSubScene != sceneName)
            yield return null;
        while (GameObject.Find("Environment") == null)
            yield return null;

        UIManager.Instance.ToggleLoadingScreen(false);
        Debug.Log("Disabling Loading Screen");
    }

    [ClientRpc]
    public void ForceMovePlayer(Vector2 pos)
    {
        if (hasAuthority)
            transform.position = pos;
    }
}
