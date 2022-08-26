using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    private string url;
    [SerializeField]
    private Text confirmationText;
    [SerializeField]
    private InputField usernameInputField, emailInputField, passwordInputField, birthdayYearInputField, birthdayMonthInputField, birthdayDayInputField;

    public void SendRegisterInfo() //to be involved by button click
    {
        //Verify that password is the same, if they are then continue, else try again
        //if (if_verifyPassword.text != if_password.text)
        //{
        //    displayTxt.text = "Passwords do not match. Please try again";
        //    return;
        //}

        //Verify that email is a proper email. If it is then continue, else try again
        if (!emailInputField.text.Contains(".com") || !emailInputField.text.Contains("@"))
        {
            confirmationText.text = "Email is not a proper email. Please try again";
            return;
        }

        //Checks birthday if they are correct
        if (CheckBirthdayInfo())
        {
            StartCoroutine(DoVerifyEmail());
        }
    }

    IEnumerator DoVerifyEmail()
    {
        url = ServerDataManager.URL_verifyEmail;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("sEmail", emailInputField.text);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                if (webreq.downloadHandler.text == "Success")
                    StartCoroutine(DoRegister());
                else
                    confirmationText.text = webreq.downloadHandler.text;
                break;
            default:
                confirmationText.text = "Server error";
                break;
        }
    }

    IEnumerator DoRegister()
    {
        url = ServerDataManager.URL_register;
        Debug.Log(url);

        //Create the birthday text
        string birthdayText = birthdayYearInputField.text + "-" + birthdayMonthInputField.text + "-" + birthdayDayInputField.text;

        Debug.Log(birthdayText);

        WWWForm form = new WWWForm();
        form.AddField("sUsername", usernameInputField.text);
        form.AddField("sEmail", emailInputField.text);
        form.AddField("sPassword", passwordInputField.text);
        form.AddField("dBirthday", birthdayText);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                confirmationText.text = webreq.downloadHandler.text;
                break;
            default:
                confirmationText.text = "server error";
                break;
        }
    }

    bool CheckBirthdayInfo()
    {
        //Checks the birthday info if they are correct.
        int birthdayYear = int.Parse(birthdayYearInputField.text);
        int birthdayMonth = int.Parse(birthdayMonthInputField.text);
        int birthdayDay = int.Parse(birthdayDayInputField.text);

        if (birthdayYear < 1000 || birthdayYear > 9999)
        {
            confirmationText.text = "Birthday Year is invalid. Try again.";
            return false;
        }

        if (birthdayMonth < 1 || birthdayMonth > 12)
        {
            confirmationText.text = "Birthday Month is invalid. Try again.";
            return false;
        }

        if (birthdayDay < 1 || birthdayDay > 31)
        {
            confirmationText.text = "Birthday Day is invalid. Try again.";
            return false;
        }

        return true;
    }
}
