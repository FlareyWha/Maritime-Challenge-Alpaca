using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashScreen : MonoBehaviour
{

    public float wait_time = 1.0f;

    void Start()
    {
        StartCoroutine(SplashcreenWait());
    }


    IEnumerator SplashcreenWait()
    {
        yield return new WaitForSeconds(wait_time);

        SceneManager.Instance.LoadScene(SCENE_ID.LOGIN);
    }
}
