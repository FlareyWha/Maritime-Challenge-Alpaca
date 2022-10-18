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
    [SerializeField]
    private AvatarDisplay DisplayAvatar;

   
    void Start()
    {
        LoadData();
        //PlayerData.OnPlayerDataUpdated += LoadData;
    }

    public void LoadData()
    {
        StartCoroutine(PlayerDataInits());
    }

    IEnumerator PlayerDataInits()
    {
        while (PlayerData.MyPlayer == null)
            yield return null;

        //Edit all the text
        EditNameText();
        EditGuildText();
        EditDepartmentText();
        EditCountryText();
        EditBirthdayText();
        EditBiographyText();
        EXPFill.fillAmount = PlayerData.CurrXP / GameSettings.GetEXPRequirement(PlayerData.CurrLevel);
        LevelNum.text = PlayerData.CurrLevel.ToString();

        PlayerAvatarManager manager = PlayerData.MyPlayer.GetComponent<PlayerAvatarManager>();
        while (!manager.IsInitted())
            yield return null; 
        DisplayAvatar.SetPlayer(PlayerData.MyPlayer);
    }

    public void EditName()
    {
        // TBC - warning text
        if (nameInputField.text == "")
            return;

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
                PlayerData.SetBio(biographyInputField.text);
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

   

    void EditNameText()
    {
        nameText.text = "Name: " + PlayerData.Name;
        nameInputField.text = PlayerData.Name;
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
        biographyInputField.text = PlayerData.Biography;
    }

    public void LogOut()
    {
        ConnectionManager.Instance.DisconnectFromServer();
        
    }
}
