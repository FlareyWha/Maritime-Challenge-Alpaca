using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private Toggle JoystickToggle;

    [SerializeField]
    private AudioMixer audioMixer;
    
    void Start()
    {
        JoystickToggle.isOn = GameSettings.LOCK_JOYSTICK;

        SetBGMVolume(0);
        SetSFXVolume(0);
    }

    public void OnJoystickToggled()
    {
        GameSettings.LOCK_JOYSTICK = JoystickToggle.isOn;
        UIManager.Instance.Joystick.OnJoystickSettingsChanged();
    }

    public void SetBGMVolume(float volume)
    {
        audioMixer.SetFloat("BGMVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFXVolume", volume);
    }

}
