using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MONOTEST : MonoBehaviour
{

    [SerializeField]
    private string[] scenesList;

    private void Start()
    {
        for (int i = 0; i < scenesList.Length; i++)
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(scenesList[i], new LoadSceneParameters { loadSceneMode = LoadSceneMode.Additive, localPhysicsMode = LocalPhysicsMode.Physics2D });
    }
}
