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

    private int UID = -1;
    private static List<AirHockeySeat> airHockeySeatsList = new List<AirHockeySeat>();


    void Start()
    {
        interactMessage = "Play Air Hockey";

        UID = airHockeySeatsList.Count;
        airHockeySeatsList.Add(this);
    }

    public override void Interact()
    {
        AirHockeyGameManager.PlayerJoinGame(this.UID, PlayerData.MyPlayer);
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

    public static AirHockeySeat GetSeat(int id)
    {
        foreach (AirHockeySeat seat in airHockeySeatsList)
        {
            if (seat.UID == id)
                return seat;
        }
        Debug.LogError("Could Not Find Seat of ID " + id);
        return null;
    }

}
