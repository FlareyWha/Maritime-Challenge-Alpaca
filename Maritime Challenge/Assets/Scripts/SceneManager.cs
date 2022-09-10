using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    // TEST
    public void EnterNetworkedScene(string sceneName)
    {
        PlayerData.CommandsHandler.EnterScene(PlayerData.MyPlayer.gameObject, sceneName);
    }

}

public enum SCENE_ID
{
    SPLASH = 0,
    LOGIN = 1,
    HOME = 2,
    CAFE = 3,

    NUM_TOTAL
}
