using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WarningPopUp : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void FixedUpdate()
    {
        if (transform.parent == null && UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LoginScene" && GameObject.Find("Canvas") != null)
        {
            transform.SetParent(GameObject.Find("Canvas").transform);
            transform.localPosition = Vector3.zero;
        }
    }
    public void OnCloseButtonClicked()
    {
        Destroy(this.gameObject);
    }
}
