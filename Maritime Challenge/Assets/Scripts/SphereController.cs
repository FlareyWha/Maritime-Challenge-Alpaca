using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    [SerializeField]
    private GameObject sphere;

    // Update is called once per frame
    void Update()
    {
        if (InputManager.InputActions.Main.Tap.WasPressedThisFrame())
        {
            Debug.Log("ADAJSDHASJD00");
            sphere.SetActive(true);
        }
        else if (InputManager.InputActions.Main.Tap.WasReleasedThisFrame())
        {
            Debug.Log("TOHKRLHKRHFGLKH");
            sphere.SetActive(false);
        }
    }
}
