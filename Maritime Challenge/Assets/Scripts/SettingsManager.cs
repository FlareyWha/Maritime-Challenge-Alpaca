using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private Toggle JoystickToggle;
    
    void Start()
    {
        JoystickToggle.isOn = GameSettings.LOCK_JOYSTICK;
    }

    public void OnJoystickToggled()
    {
        GameSettings.LOCK_JOYSTICK = JoystickToggle.isOn;
        UIManager.Instance.Joystick.OnJoystickSettingsChanged();
    }



}
