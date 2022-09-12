using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    public static Controls InputActions;

    void Start()
    {
        InputActions = new Controls();

        //Scene scene = SceneManager.GetActiveScene();

        SwitchActionMap(InputActions.Main);

        //if (scene.name == "Main Menu")
        //    SwitchActionMap(InputActions.MainMenu);
        //else if (scene.name == "Exploration")
        //    SwitchActionMap(InputActions.PlayerExplore);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SwitchActionMap(InputActionMap actionMap)
    {
        //Doesnt allow switching to the same action map
        if (actionMap.enabled)
            return;

        InputActions.Disable();
        actionMap.Enable();
    }


}
