using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Register : MonoBehaviour
{
    [SerializeField]
    private Text confirmationText;
    [SerializeField]
    private InputField usernameInputField, emailInputField, passwordInputField, birthdayYearInputField, birthdayMonthInputField, birthdayDayInputField;
    [SerializeField]
    private Dropdown genderDropdown;

    [SerializeField]
    private RefreshDatabaseManager refreshDatabaseManager;

    [SerializeField]
    private GameObject fields, loadingScreenSpin;

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

    public void SendRegisterInfo() //to be involved by button click
    {
        confirmationText.text = "";

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
        string url = ServerDataManager.URL_verifyEmail;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("sEmail", emailInputField.text);

        //url += "sEmail=" + UnityWebRequest.EscapeURL(emailInputField.text);
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
        string url = ServerDataManager.URL_register;
        Debug.Log(url);

        fields.SetActive(false);
        loadingScreenSpin.SetActive(true);

        //Create the birthday text
        string birthdayText = birthdayYearInputField.text + "-" + birthdayMonthInputField.text + "-" + birthdayDayInputField.text;

        Debug.Log(birthdayText);

        WWWForm form = new WWWForm();
        form.AddField("sUsername", usernameInputField.text);
        form.AddField("sEmail", emailInputField.text);
        form.AddField("sPassword", passwordInputField.text);
        form.AddField("dBirthday", birthdayText);
        form.AddField("iGender", genderDropdown.value);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                yield return StartCoroutine(refreshDatabaseManager.DoRefreshDatabase());
                yield return StartCoroutine(GetUID());
                confirmationText.text = webreq.downloadHandler.text;
                break;
            case UnityWebRequest.Result.ProtocolError:
                confirmationText.text = webreq.downloadHandler.text;
                break;
            default:
                confirmationText.text = "server error";
                break;
        }
        fields.SetActive(true);
        loadingScreenSpin.SetActive(false);
    }

    IEnumerator GetUID()
    {
        //Set the URL to the getUID one
        string url = ServerDataManager.URL_login;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("sEmail", emailInputField.text);
        form.AddField("sPassword", passwordInputField.text);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                int uid = int.Parse(webreq.downloadHandler.text);
                yield return StartCoroutine(UpdateData(uid));
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.Log(webreq.downloadHandler.text);
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator UpdateData(int uid)
    {
        CoroutineCollection coroutineCollectionManager = new CoroutineCollection();

        StartCoroutine(coroutineCollectionManager.CollectCoroutine(AddPlayerStats(uid)));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(UnlockDefaultCosmetics(uid)));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(SetDefaultEquippedCosmetics(uid)));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(UnlockDefaultBattleship(uid)));
        StartCoroutine(coroutineCollectionManager.CollectCoroutine(UnlockDefaultTitle(uid)));

        //Wait for all the coroutines to finish running before continuing
        yield return coroutineCollectionManager;
    }

    IEnumerator AddPlayerStats(int uid)
    {
        string url = ServerDataManager.URL_addPlayerStats;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("iOwnerUID", uid);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.Log(webreq.downloadHandler.text);
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator UnlockDefaultCosmetics(int uid)
    {
        string url = ServerDataManager.URL_updateDefaultUnlockedCosmetics;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", uid);
        form.AddField("iGender", genderDropdown.value);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.Log(webreq.downloadHandler.text);
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator SetDefaultEquippedCosmetics(int uid)
    {
        string url = ServerDataManager.URL_addDefaultEquippedCosmetics;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", uid);
        form.AddField("iGender", genderDropdown.value);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.Log(webreq.downloadHandler.text);
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator UnlockDefaultBattleship(int uid)
    {
        string url = ServerDataManager.URL_updateBattleshipList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", uid);
        form.AddField("iBattleshipID", 1);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.Log(webreq.downloadHandler.text);
                Debug.LogError("Server error");
                break;
        }
    }

    IEnumerator UnlockDefaultTitle(int uid)
    {
        string url = ServerDataManager.URL_updateTitleList;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", uid);
        form.AddField("iTitleID", 1);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                Debug.Log(webreq.downloadHandler.text);
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.Log(webreq.downloadHandler.text);
                Debug.LogError("Server error");
                break;
        }
    }
}
