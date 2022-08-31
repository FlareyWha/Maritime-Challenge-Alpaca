using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProfileNamecard : MonoBehaviour
{
    [SerializeField]
    private Text Name, Guild, Country, Bio, Level;
    [SerializeField]
    private Image Title;


    public void SetDetails(Player player)
    {
        Name.text = player.GetUsername();
        Bio.text = player.GetBio();
        Level.text = player.GetLevel().ToString();
        Country.text = PlayerData.GetCountryName(player.GetCountryID());
        Title.sprite = PlayerData.GetTitleByID(player.GetTitleID());

    }

    public void SetDetails(FriendInfo player)
    {
        Name.text = player.Name;
        Bio.text = player.Biography;
        Level.text = player.CurrLevel.ToString();
        Country.text = PlayerData.GetCountryName(player.Country);
        Title.sprite = PlayerData.GetTitleByID(player.CurrentTitleID);

    }

}
