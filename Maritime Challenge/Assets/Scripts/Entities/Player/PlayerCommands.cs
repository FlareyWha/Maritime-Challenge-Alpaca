using UnityEngine;
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

   
    [ClientRpc]
    public void ForceMovePlayer(Vector2 pos)
    {
        if (hasAuthority)
            transform.position = pos;
    }
}
