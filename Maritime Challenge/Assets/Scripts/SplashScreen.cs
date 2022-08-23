using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{

    public float wait_time = 1.0f;

    public Image splashImage;

    void Start()
    {
        StartCoroutine(SplashcreenWait());
    }

    void Update()
    {
        Vector3 currRot = splashImage.transform.rotation.eulerAngles;
        currRot.z += 200.0f * Time.deltaTime;
        currRot.z = Mathf.Clamp(currRot.z, 0.0f, 360.0f);
        splashImage.transform.rotation = Quaternion.Euler(currRot);
    }

    IEnumerator SplashcreenWait()
    {
        yield return new WaitForSeconds(wait_time);

        SceneManager.Instance.LoadScene(SCENE_ID.LOGIN);
    }
}
