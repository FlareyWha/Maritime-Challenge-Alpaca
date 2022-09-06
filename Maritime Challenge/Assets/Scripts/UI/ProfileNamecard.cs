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

    private int playerID = 0;


    public void SetDetails(Player player)
    {
        playerID = player.GetUID();

        ProfileInfo.SetActive(true);
        HiddenPanel.SetActive(false);
        UnknownPanel.SetActive(false);
        Name.text = "Name: " + player.GetUsername();
        Bio.text = player.GetBio();
        Level.text = player.GetLevel().ToString();
        Country.text = "Country: " + PlayerData.GetCountryName(player.GetCountryID());
        Guild.text = "Guild: " + PlayerData.GetGuildName(player.GetGuildID());
        //Title.sprite = PlayerData.GetTitleByID(player.GetTitleID());

    }

    public void SetDetails(FriendInfo player)
    {
        playerID = player.UID;

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

    public void SetHidden(int playerID)
    {
        this.playerID = playerID;

        ProfileInfo.SetActive(false);
        HiddenPanel.SetActive(true);
        UnknownPanel.SetActive(false);

    }

    public void SetUnknown(int playerID)
    {
        this.playerID = playerID;

        ProfileInfo.SetActive(false);
        HiddenPanel.SetActive(false);
        UnknownPanel.SetActive(true);

        AvatarImage.sprite = UnknownSprite;
    }

    public int GetPlayerID()
    {
        return playerID;
    }

}
