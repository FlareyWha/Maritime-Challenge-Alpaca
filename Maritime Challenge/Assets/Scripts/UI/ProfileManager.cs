using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    private Text nameText, guildText, departmentText, countryText, birthdayText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadData()
    {
        //Edit all the text
        EditNameText();
        EditGuildText();
        EditDepartmentText();
        EditCountryText();
        EditBirthdayText();
    }

    void EditNameText()
    {
        nameText.text = "Name: " + PlayerData.Name;
    }

    void EditGuildText()
    {
        //Get guild name based off id wait does this mean i need another table AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
        guildText.text = "Guild: " + PlayerData.GuildID;
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

        switch (PlayerData.Country)
        {
            case 0:
                countryName = "Singapore";
                break;
            case 1:
                countryName = "America";
                break;
            case 2:
                countryName = "Australia";
                break;
            case 3:
                countryName = "Europe";
                break;
        }

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
