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
    private GameObject ProfileInfo, HiddenPanel, UnknownPanel, PendingPanel, IncomingPanel;
    [SerializeField]
    private Button AcceptFriendRequestButton, AddFriendButton, RemoveFriendButton;
  
    // ProfileInfo - Friends
    // Hidden - Not Friends
    // Pending - Sent Friend Request to player
    // Incoming - Received Friend Request from player
    // Unknown - Have not met/unlocked

    [SerializeField]
    private AvatarDisplay DisplayAvatar;

    private int playerID = 0;

    void Awake()
    {
        AddFriendButton.onClick.AddListener(OnAddFriendButtonClicked);
        RemoveFriendButton.onClick.AddListener(OnRemoveFriendButtonClicked);
        AcceptFriendRequestButton.onClick.AddListener(OnAcceptButtonClicked);
    }

    public void SetDetails(Player player)
    {
        // Player
        playerID = player.GetUID();

        // Namecard Panels/Types
        ProfileInfo.SetActive(true);
        HiddenPanel.SetActive(false);
        UnknownPanel.SetActive(false);
        PendingPanel.SetActive(false);
        IncomingPanel.SetActive(false);

        // Player Details
        Name.text = "Name: " + player.GetUsername();
        Bio.text = player.GetBio();
        Level.text = player.GetLevel().ToString();
        Country.text = "Country: " + PlayerData.GetCountryName(player.GetCountryID());
        Guild.text = "Guild: " + PlayerData.GetGuildName(player.GetGuildID());
        Title.sprite = TitleManager.Instance.FindTitleByID(player.GetTitleID()).TitleSprite;
        // AvatarImage.sprite = DefaultSprite;

        // Avatar Display
        DisplayAvatar.SetPlayer(player);

    }

    public void SetDetails(FriendInfo player)
    {
        playerID = player.UID;

        // Namecard Panels/Types
        ProfileInfo.SetActive(true);
        HiddenPanel.SetActive(false);
        UnknownPanel.SetActive(false);
        PendingPanel.SetActive(false);
        IncomingPanel.SetActive(false);
        // Player Details
        Name.text = "Name: " + player.Name;
        Bio.text = player.Biography;
        Level.text = player.CurrLevel.ToString();
        Country.text = "Country: " + PlayerData.GetCountryName(player.Country);
        Guild.text = "Guild: " + PlayerData.GetGuildName(player.GuildID);
        Title.sprite = TitleManager.Instance.FindTitleByID(player.CurrentTitleID).TitleSprite;
        //AvatarImage.sprite = DefaultSprite;

        // Avatar Display
        DisplayAvatar.SetPlayer(player.UID);
    }

    // For Online
    public void SetHidden(Player player)
    {
        this.playerID = player.GetUID();
        Name.text = "Name: " + PlayerData.FindPlayerNameByID(playerID);

        ProfileInfo.SetActive(false);
        UnknownPanel.SetActive(false);

        if (FriendsManager.CheckIfPending(playerID))
        {
            HiddenPanel.SetActive(false);
            PendingPanel.SetActive(true);
            IncomingPanel.SetActive(false);

        }
        else if (FriendsManager.CheckIfIncoming(playerID))
        {
            HiddenPanel.SetActive(false);
            PendingPanel.SetActive(false);
            IncomingPanel.SetActive(true);
        }
        else
        {
            HiddenPanel.SetActive(true);
            PendingPanel.SetActive(false);
            IncomingPanel.SetActive(false);
        }

        //AvatarImage.sprite = DefaultSprite;
        DisplayAvatar.SetPlayer(player);
    }

    // For Offline
    public void SetHidden(BasicInfo playerInfo)
    {
        this.playerID = playerInfo.UID;
        Name.text = "Name: " + playerInfo.Name;

        ProfileInfo.SetActive(false);
        UnknownPanel.SetActive(false);

        if (FriendsManager.CheckIfPending(playerID))
        {
            HiddenPanel.SetActive(false);
            PendingPanel.SetActive(true);
            IncomingPanel.SetActive(false);

        }
        else if (FriendsManager.CheckIfIncoming(playerID))
        {
            HiddenPanel.SetActive(false);
            PendingPanel.SetActive(false);
            IncomingPanel.SetActive(true);
        }
        else
        {
            HiddenPanel.SetActive(true);
            PendingPanel.SetActive(false);
            IncomingPanel.SetActive(false);
        }

        //AvatarImage.sprite = DefaultSprite;
        DisplayAvatar.SetPlayer(playerInfo.UID);
    }

    public void SetUnknown(int playerID)
    {
        this.playerID = playerID;
        Name.text = "";

        ProfileInfo.SetActive(false);
        HiddenPanel.SetActive(false);
        PendingPanel.SetActive(false);
        UnknownPanel.SetActive(true);

        DisplayAvatar.SetUnknown();
    }


    private void OnAcceptButtonClicked()
    {
        FriendsManager.Instance.AddFriend(playerID, Name.text);
        FriendsManager.Instance.DeleteFriendRequest(playerID, PlayerData.UID);
    }

    private void OnAddFriendButtonClicked()
    {
        FriendsManager.Instance.SendFriendRequest(playerID);
    }

    private void OnRemoveFriendButtonClicked()
    {
        FriendsManager.Instance.DeleteFriend(playerID);
    }

    public int GetPlayerID()
    {
        return playerID;
    }

}
