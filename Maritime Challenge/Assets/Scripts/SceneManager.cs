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
    public void EnterNetworkedSubScene(NetworkIdentity playerNetIdentity, string sceneName)
    {
        SceneMessage message = new SceneMessage { sceneName = sceneName, sceneOperation = SceneOperation.LoadAdditive };
        playerNetIdentity.connectionToClient.Send(message);
    }

    //TEST-FOR INSPECTER
    public void RequestEnterScene(string sceneName)
    {
        PlayerData.CommandsHandler.RequestEnterScene(sceneName);
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
