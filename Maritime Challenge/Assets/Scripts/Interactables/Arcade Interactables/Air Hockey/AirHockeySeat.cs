using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirHockeySeat : Interactable
{
    [SerializeField]
    private AirHockeyMinigame AirHockeyGameManager;
    [SerializeField]
    private AirHockeyPaddle PlayerPaddle;
    [SerializeField]
    private bool IsOppositeSide;

    //public bool OnOppositeSide { get { return IsOppositeSide; } }

    private int UID = -1;

    void Start()
    {
        interactMessage = "Play Air Hockey";

        UID = AirHockeyGameManager.GetAssignedSeatID(this);
    }

    public override void Interact()
    {
        AirHockeyGameManager.PlayerJoinGame(this.UID, IsOppositeSide, PlayerData.MyPlayer);
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

}
