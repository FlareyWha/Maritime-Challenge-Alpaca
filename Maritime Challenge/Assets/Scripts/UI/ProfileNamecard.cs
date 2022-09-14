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
    private Image AvatarImage;
    [SerializeField]
    private Sprite UnknownSprite, DefaultSprite;

    private int playerID = 0;

    void Awake()
    {
        AddFriendButton.onClick.AddListener(OnAddFriendButtonClicked);
        RemoveFriendButton.onClick.AddListener(OnRemoveFriendButtonClicked);
        AcceptFriendRequestButton.onClick.AddListener(OnAcceptButtonClicked);
    }

    public void SetDetails(Player player)
    {
        playerID = player.GetUID();

        ProfileInfo.SetActive(true);
        HiddenPanel.SetActive(false);
        UnknownPanel.SetActive(false);
        PendingPanel.SetActive(false);
        IncomingPanel.SetActive(false);
        Name.text = "Name: " + player.GetUsername();
        Bio.text = player.GetBio();
        Level.text = player.GetLevel().ToString();
        Country.text = "Country: " + PlayerData.GetCountryName(player.GetCountryID());
        Guild.text = "Guild: " + PlayerData.GetGuildName(player.GetGuildID());
        //Title.sprite = PlayerData.GetTitleByID(player.GetTitleID());
        AvatarImage.sprite = DefaultSprite;
    }

    public void SetDetails(FriendInfo player)
    {
        playerID = player.UID;

        ProfileInfo.SetActive(true);
        HiddenPanel.SetActive(false);
        UnknownPanel.SetActive(false);
        PendingPanel.SetActive(false);
        IncomingPanel.SetActive(false);
        Name.text = "Name: " + player.Name;
        Bio.text = player.Biography;
        Level.text = player.CurrLevel.ToString();
        Country.text = "Country: " + PlayerData.GetCountryName(player.Country);
        Guild.text = "Guild: " + PlayerData.GetGuildName(player.GuildID);
        //Title.sprite = PlayerData.GetTitleByID(player.CurrentTitleID);
        AvatarImage.sprite = DefaultSprite;
    }

    public void SetHidden(int playerID)
    {
        this.playerID = playerID;
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

        AvatarImage.sprite = DefaultSprite;

    }

    public void SetUnknown(int playerID)
    {
        this.playerID = playerID;
        Name.text = "";

        ProfileInfo.SetActive(false);
        HiddenPanel.SetActive(false);
        PendingPanel.SetActive(false);
        UnknownPanel.SetActive(true);

        AvatarImage.sprite = UnknownSprite;
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
