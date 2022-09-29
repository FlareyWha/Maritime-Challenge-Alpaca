using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameHandler : NetworkBehaviour
{
    #region Singleton
    public static GameHandler Instance = null;
    #endregion
    private readonly SyncList<Player> onlinePlayers = new SyncList<Player>();

    private void Awake()
    {
        Instance = this;
    }

    [Server]
    public void OnNewPlayerJoined(NetworkConnectionToClient conn)
    {
        StartCoroutine(SetPlayerOnline(conn));
    }
    IEnumerator SetPlayerOnline(NetworkConnectionToClient conn)
    {
        while (conn.identity == null)
            yield return null;
        onlinePlayers.Add(conn.identity.gameObject.GetComponent<Player>());
    }
    [Server]
    public void OnPlayerQuit(NetworkConnectionToClient conn)
    {
        onlinePlayers.Remove(conn.identity.gameObject.GetComponent<Player>());
    }


    [ClientRpc]
    public void OnGameObjectMoved(GameObject go, string sceneName)
    {
        //check if scene valid
        SceneManager.Instance.MoveGameObjectToScene(go, sceneName);
    }

    [Command]
    public void RequestMoveGameObjectToScene(GameObject go, string sceneName)
    {
        SceneManager.Instance.GlobalMoveGameObjectToScene(go, sceneName);
    }


}
