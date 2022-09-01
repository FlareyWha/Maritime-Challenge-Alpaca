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
    [SerializeField]
    private GameObject ProfileInfo, HiddenPanel, UnknownPanel;
    //[SerializeField]
    //private GameObject AddFriendButton;
    [SerializeField]
    private Image AvatarImage;
    [SerializeField]
    private Sprite UnknownSprite;


    public void SetDetails(Player player)
    {
        ProfileInfo.SetActive(true);
        HiddenPanel.SetActive(false);
        UnknownPanel.SetActive(false);
        Name.text = player.GetUsername();
        Bio.text = player.GetBio();
        Level.text = player.GetLevel().ToString();
        Country.text = PlayerData.GetCountryName(player.GetCountryID());
        //Title.sprite = PlayerData.GetTitleByID(player.GetTitleID());

      
    }

    public void SetDetails(FriendInfo player)
    {
        ProfileInfo.SetActive(true);
        HiddenPanel.SetActive(false);
        UnknownPanel.SetActive(false);
        Name.text = "Name: " + player.Name;
        Bio.text = player.Biography;
        Level.text = player.CurrLevel.ToString();
        Country.text = "Country: " + PlayerData.GetCountryName(player.Country);
        Guild.text = "Guild: " + PlayerData.GetGuildName(player.GuildID);
        //Title.sprite = PlayerData.GetTitleByID(player.CurrentTitleID);
      
    }

    public void SetHidden()
    {
        ProfileInfo.SetActive(false);
        HiddenPanel.SetActive(true);
        UnknownPanel.SetActive(false);

    }

    public void SetUnknown()
    {
        ProfileInfo.SetActive(false);
        HiddenPanel.SetActive(false);
        UnknownPanel.SetActive(true);

        AvatarImage.sprite = UnknownSprite;
    }

}
