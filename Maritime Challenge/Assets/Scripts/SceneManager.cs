using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class SceneManager : MonoBehaviourSingleton<SceneManager>
{

    public static Vector2 StartWorldHubSpawnPos = new Vector2(6.0f, -8.0f);
    public static Vector2 EnterCafeSpawnPos = new Vector2(0.0f, 4.0f);
    public static Vector2 EnterArcadeSpawnPos = new Vector2(0.0f, 4.0f);
    public static Vector2 EnterMallSpawnPos = new Vector2(0.0f, 4.0f);
    public static Vector2 ExitCafeSpawnPos = new Vector2(-12.0f, -8.0f);
    public static Vector2 ExitArcadeSpawnPos = new Vector2(-12.0f, -8.0f);
    public static Vector2 ExitMallSpawnPos = new Vector2(-12.0f, -8.0f);

    private void Start()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive && PlayerData.activeSubScene != scene.name)
        {
            PlayerData.activeSubScene = scene.name;
        }
    }

    public void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void LoadScene(SCENE_ID id)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)id);
    }

    [Server]
    public void EnterNetworkedSubScene(NetworkIdentity playerNetIdentity, string currSceneName, string newSceneName, Vector2 spawnPos)
    {
        //// Unload Current SubScene
        SceneMessage message = new SceneMessage { sceneName = currSceneName, sceneOperation = SceneOperation.UnloadAdditive };
        playerNetIdentity.connectionToClient.Send(message);
        //// Load New SubScene
        message = new SceneMessage { sceneName = newSceneName, sceneOperation = SceneOperation.LoadAdditive };
        playerNetIdentity.connectionToClient.Send(message);
        // Then move the player object to the subscene
        MoveGameObjectToScene(playerNetIdentity.gameObject, newSceneName);
        MoveGameObjectToScene(playerNetIdentity.gameObject.GetComponent<Player>().BattleshipGO, newSceneName);
        // Reposition Player
        playerNetIdentity.gameObject.GetComponent<PlayerCommands>().ForceMovePlayer(spawnPos);
    }

   
    [Server]
    public void MoveGameObjectToScene(GameObject go, string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go,
            UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
    }
 
}

public enum SCENE_ID
{
    SPLASH = 0,
    LOGIN = 1,
    // MAIN EMPTY SCENE
    MAIN_EMPTY = 2,
    // SUB SCENE START
    HOME = 3,
    CAFE,
    ARCADE,
    SHOPPINGMALL,

    NUM_TOTAL
}
