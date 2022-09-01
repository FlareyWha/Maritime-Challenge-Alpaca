using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PasswordShowHide : MonoBehaviour
{
    [SerializeField]
    private InputField passwordInputField;

    public void TogglePasswordVisibility()
    {
        //Toggles between the 2 depending on which type it currently is
        if (passwordInputField.contentType == InputField.ContentType.Password)
            passwordInputField.contentType = InputField.ContentType.Standard;
        else
            passwordInputField.contentType = InputField.ContentType.Password;

        passwordInputField.ForceLabelUpdate();
    }
}
