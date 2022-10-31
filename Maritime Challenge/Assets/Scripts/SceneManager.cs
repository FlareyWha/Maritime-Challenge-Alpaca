using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class SceneManager : MonoBehaviourSingleton<SceneManager>
{

    public static Vector2 StartWorldHubSpawnPos = new Vector2(6.0f, -8.0f);
    public static Vector2 TutorialSpawnPos = new Vector2(0.0f, 0.0f);
 
    public static Vector2[] SpawnPosList = new Vector2[(int)SPAWN_POS.NUM_TOTAL];


    protected override void Awake()
    {
        base.Awake();
        SpawnPosList[(int)SPAWN_POS.CAFE_ENTRANCE].Set(0.0f, 9.0f);
        SpawnPosList[(int)SPAWN_POS.ARCADE_ENTRANCE].Set(7.0f, 2.0f);
        SpawnPosList[(int)SPAWN_POS.SHOPPINGMALL_ENTRANCE].Set(-2.1f, -0.4f);
        SpawnPosList[(int)SPAWN_POS.WORLDHUB_CAFE_FRONT].Set(-2.9f, -8.0f);
        SpawnPosList[(int)SPAWN_POS.WORLDHUB_ARCADE_FRONT].Set(9.0f, 0.0f);
        SpawnPosList[(int)SPAWN_POS.WORLDHUB_SHOPPINGMALL_FRONT].Set(21.0f, -7.6f);
    }
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
        SceneMessage message = new SceneMessage { sceneName = currSceneName, sceneOperation = SceneOperation.UnloadAdditive, customHandling = true};
        playerNetIdentity.connectionToClient.Send(message);
        // Then move the player object to the subscene
        MoveGameObjectToScene(playerNetIdentity.gameObject, newSceneName);
        MoveGameObjectToScene(playerNetIdentity.gameObject.GetComponent<Player>().BattleshipGO, newSceneName);
        // Reposition Player
        playerNetIdentity.gameObject.GetComponent<PlayerCommands>().ForceMovePlayer(spawnPos);
        //// Load New SubScene
        message = new SceneMessage { sceneName = newSceneName, sceneOperation = SceneOperation.LoadAdditive, customHandling = true };
        playerNetIdentity.connectionToClient.Send(message);
    }

   
    [Server]
    public void MoveGameObjectToScene(GameObject go, string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go,
            UnityEngine.SceneManagement.SceneManager.GetSceneByName(sceneName));
    }

    public static Vector2 GetSpawnPos(SPAWN_POS posType)
    {
        return SpawnPosList[(int)posType];
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

public enum SPAWN_POS // (World/SubScene_Where)
{
    WORLDHUB_CAFE_FRONT,
    WORLDHUB_ARCADE_FRONT,
    WORLDHUB_SHOPPINGMALL_FRONT,
    CAFE_ENTRANCE,
    ARCADE_ENTRANCE,
    SHOPPINGMALL_ENTRANCE,


    NUM_TOTAL
}
