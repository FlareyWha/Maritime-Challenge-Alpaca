using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class AirHockeySeat : BaseInteractable
{
    [SerializeField]
    private AirHockeyMinigame AirHockeyGameManager;
    [SerializeField]
    private AirHockeyPaddle PlayerPaddle;
    [SerializeField]
    private AirHockeyGoal LinkedGoal;
    [SerializeField]
    private Text PlayerNameText, PlayerScoreText;
    [SerializeField]
    private bool IsOppositeSide;

    public bool OnOppositeSide { get { return IsOppositeSide; } }

    private int UID = -1;
    private bool isTaken = false;
    public bool IsTaken {  get { return isTaken; } }

    void Start()
    {
        interactMessage = "Play Air Hockey";

        UID = AirHockeyGameManager.GetAssignedSeatID(this);
        LinkedGoal.SetSeatID(UID);
    }

    public override void Interact()
    {
        AirHockeyGameManager.PlayerJoinGame(this.UID, PlayerData.MyPlayer);
    }

    public void SetIsTaken(bool taken)
    {
        isTaken = taken;
    }

    protected override bool CheckRequirements()
    {
        return !isTaken;
    }


    [Server]
    public void AssignPaddleControl(Player player)
    {
        Debug.Log("Assigning Authority to " + player.GetUsername());
        PlayerPaddle.AssignController(player.netIdentity);
    }

    [Server]
    public void RevokePaddleControl()
    {
        PlayerPaddle.RevokeControl();
    }


    public void UpdateScoreDisplay(int score)
    {
        PlayerScoreText.text = score.ToString();
    }


    public void UpdateNameDisplay(string text)
    {
        PlayerNameText.text = text;
    }

}
