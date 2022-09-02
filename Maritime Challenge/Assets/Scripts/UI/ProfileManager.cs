using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    private Text nameText, guildText, departmentText, countryText, birthdayText;
    [SerializeField]
    private Image EXPFill;
    [SerializeField]
    private Text LevelNum;

   
    void Start()
    {
        LoadData();
    }

    public void LoadData()
    {
        //Edit all the text
        EditNameText();
        EditGuildText();
        EditDepartmentText();
        EditCountryText();
        EditBirthdayText();
        EXPFill.fillAmount = PlayerData.CurrXP / GameSettings.GetEXPRequirement(PlayerData.CurrLevel);
        LevelNum.text = PlayerData.CurrLevel.ToString();
    }

    void EditNameText()
    {
        nameText.text = "Name: " + PlayerData.Name;
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
        string countryName = "";

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
}
