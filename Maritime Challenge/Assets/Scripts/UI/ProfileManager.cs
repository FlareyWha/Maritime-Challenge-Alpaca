using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    private Text nameText, guildText, departmentText, countryText, birthdayText, biographyText;
    [SerializeField]
    private InputField nameInputField, biographyInputField;
    [SerializeField]
    private Image EXPFill;
    [SerializeField]
    private Text LevelNum;

   
    void Start()
    {
        LoadData();

        //PlayerData.OnPlayerDataUpdated += LoadData;
    }

    public void LoadData()
    {
        //Edit all the text
        EditNameText();
        EditGuildText();
        EditDepartmentText();
        EditCountryText();
        EditBirthdayText();
        EditBiographyText();
        EXPFill.fillAmount = PlayerData.CurrXP / GameSettings.GetEXPRequirement(PlayerData.CurrLevel);
        LevelNum.text = PlayerData.CurrLevel.ToString();
    }

    public void EditName()
    {
        StartCoroutine(StartEditName());
    }

    IEnumerator StartEditName()
    {
        string url = ServerDataManager.URL_updateUsername;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("sUsername", nameInputField.text);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                Debug.Log(webreq.downloadHandler.text);
                PlayerData.SetUsername(nameInputField.text);
                EditNameText();
                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public void EditBiography()
    {
        StartCoroutine(StartEditBiography());
    }

    IEnumerator StartEditBiography()
    {
        string url = ServerDataManager.URL_updateBiography;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("sBiography", biographyInputField.text);
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                Debug.Log(webreq.downloadHandler.text);

                EditBiographyText();

                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    public void EditTitle()
    {
        StartCoroutine(StartEditTitle());
    }

    IEnumerator StartEditTitle()
    {
        string url = ServerDataManager.URL_updateTitle;
        Debug.Log(url);

        WWWForm form = new WWWForm();
        form.AddField("UID", PlayerData.UID);
        form.AddField("iTitle", 0); //Change later btw
        using UnityWebRequest webreq = UnityWebRequest.Post(url, form);
        yield return webreq.SendWebRequest();
        switch (webreq.result)
        {
            case UnityWebRequest.Result.Success:
                //Deseralize the data
                Debug.Log(webreq.downloadHandler.text);

                EditTitle();

                break;
            case UnityWebRequest.Result.ProtocolError:
                Debug.LogError(webreq.downloadHandler.text);
                break;
            default:
                Debug.LogError("Server error");
                break;
        }
    }

    void EditNameText()
    {
        nameText.text = "Name: " + PlayerData.Name;
        nameInputField.text = "";
    }

    void EditGuildText()
    {
        //Get guild name based off id wait does this mean i need another table AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        // i created func its FINE BUDDY
        guildText.text = "Guild: " + PlayerData.GetGuildName(PlayerData.GuildID);
    }

    void EditDepartmentText()
    {
        //Honestly do i need another table AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        string departmentName = "";

        switch (PlayerData.Department)
        {
            case 0:
                departmentName = "Security";
                break;
        }

        departmentText.text = "Department: " + departmentName;
    }
    
    void EditCountryText()
    {
        string countryName;

        countryName = PlayerData.GetCountryName(PlayerData.Country);

        countryText.text = "Country: " + countryName;
    }

    void EditBirthdayText()
    {
        if (PlayerData.ShowBirthday)
            birthdayText.text = "Birthday: " + PlayerData.Birthday;
        else
            birthdayText.text = "Birthday: Hidden";
    }

    void EditBiographyText()
    {
        biographyText.text = PlayerData.Biography;
    }

    public void LogOut()
    {
        ServerManager.Instance.DisconnectFromServer();
        
    }
}
