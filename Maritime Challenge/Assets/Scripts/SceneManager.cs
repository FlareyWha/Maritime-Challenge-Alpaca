using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class SceneManager : MonoBehaviourSingleton<SceneManager>
{


    public void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(name);
    }

    public void LoadScene(SCENE_ID id)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene((int)id);
    }

    [Server]
    public void EnterNetworkedSubScene(NetworkIdentity playerNetIdentity, string currSceneName, string newSceneName)
    {
        // Unload Current SubScene
        SceneMessage message = new SceneMessage { sceneName = currSceneName, sceneOperation = SceneOperation.UnloadAdditive };
        playerNetIdentity.connectionToClient.Send(message);
        // Load New SubScene
        message = new SceneMessage { sceneName = newSceneName, sceneOperation = SceneOperation.LoadAdditive };
        playerNetIdentity.connectionToClient.Send(message);
        // Then move the player object to the subscene
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(playerNetIdentity.gameObject,
            UnityEngine.SceneManagement.SceneManager.GetSceneByName(newSceneName));
        // Reposition Player
        playerNetIdentity.gameObject.transform.position = Vector3.zero;
    }

    [Server]
    public void EnterNetworkedScene(NetworkIdentity playerNetIdentity, string newSceneName)
    {
        // Load SubScene
        SceneMessage message = new SceneMessage { sceneName = newSceneName, sceneOperation = SceneOperation.LoadAdditive };
        playerNetIdentity.connectionToClient.Send(message);
        // Then move the player object to the subscene
        UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(playerNetIdentity.gameObject,
            UnityEngine.SceneManagement.SceneManager.GetSceneByName(newSceneName));
        // Reposition Player
        playerNetIdentity.gameObject.transform.position = new Vector3(0, 1, 0);
    }

    //TEST-FOR INSPECTER
    public void RequestEnterScene(string sceneName)
    {
        PlayerData.CommandsHandler.SwitchSubScene(sceneName);
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

    NUM_TOTAL
}

public enum SERVER_SCENE_ID
{
    SERVER = 0,
    SPLASH,
    LOGIN,
    // MAIN EMPTY SCENE
    MAIN_EMPTY = 3,
    // SUB SCENE START
    HOME = 4,
    CAFE,

    NUM_TOTAL
}
