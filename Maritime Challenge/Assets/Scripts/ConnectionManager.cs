using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using UnityEngine.Networking;

public class ConnectionManager : NetworkManager
{
    #region Singleton
    public static ConnectionManager Instance = null;
    #endregion

    private bool offlineSetted = false;
    public bool OfflineSetted
    {
        get { return offlineSetted; }
        set { offlineSetted = value; }
    }

   // private NetworkManager manager;

    public override void Start()
    {
        Instance = this;
       // manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }


    public void ConnectToServer()
    {
        StartClient();
    }


    public void DisconnectFromServer()
    {
        StopClient();
    }


    public override void OnClientChangeScene(string newSceneName, SceneOperation sceneOperation, bool customHandling) 
    {
        if (!customHandling)
            return;

        if (sceneOperation == SceneOperation.UnloadAdditive)
            StartCoroutine(UnloadAdditive(newSceneName));

        if (sceneOperation == SceneOperation.LoadAdditive)
            StartCoroutine(LoadAdditive(newSceneName));
    }

    IEnumerator LoadAdditive(string sceneName)
    {
        UIManager.Instance.ToggleLoadingScreen(true);

        // host client is on server...don't load the additive scene again
        if (mode == NetworkManagerMode.ClientOnly)
        {
            // Start loading the additive subscene
            loadingSceneAsync = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

            while (loadingSceneAsync != null && !loadingSceneAsync.isDone)
                yield return null;
        }

        // Reset these to false when ready to proceed
        NetworkClient.isLoadingScene = false;

        OnClientSceneChanged();

        while (GameObject.Find("Environment") == null)
            yield return null;

        UIManager.Instance.ToggleLoadingScreen(false);

    }

    IEnumerator UnloadAdditive(string sceneName)
    {
        UIManager.Instance.ToggleLoadingScreen(true);

        if (mode == NetworkManagerMode.ClientOnly)
        {
            yield return UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);
            yield return Resources.UnloadUnusedAssets();
        }

        NetworkClient.isLoadingScene = false;
        OnClientSceneChanged();

    
        Debug.Log("Unloading Additive Finished");
    }

    [Client]
    public static Player GetPlayerByNetID(uint playerNetID)
    {
        if (NetworkClient.spawned.TryGetValue(playerNetID, out NetworkIdentity identity))
            return identity.gameObject.GetComponent<Player>();
        else
            return null;
    }

    public override void OnClientDisconnect()
    {
        base.OnClientDisconnect();

        PlayerData.OnExitSaveData();
    }

    public override void OnApplicationQuit()
    {
        base.OnApplicationQuit();

        if (isNetworkActive)
            PlayerData.OnExitSaveData();
    }
}
