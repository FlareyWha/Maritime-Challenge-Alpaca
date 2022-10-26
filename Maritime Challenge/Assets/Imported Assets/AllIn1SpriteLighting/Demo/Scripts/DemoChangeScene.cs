using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoChangeScene : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            NextScene();
        }   
    }

    public void NextScene()
    {
        if (UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings <= UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void LoadScene(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }
}