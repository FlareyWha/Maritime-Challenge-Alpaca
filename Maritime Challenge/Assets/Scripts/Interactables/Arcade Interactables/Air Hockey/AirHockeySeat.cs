using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirHockeySeat : Interactable
{
    [SerializeField]
    private AirHockeyMinigame AirHockeyGame;

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
        AirHockeyGame.PlayerJoinGame(this.UID, PlayerData.MyPlayer);
    }

    public static AirHockeySeat GetSeat(int id)
    {
        foreach (AirHockeySeat seat in airHockeySeatsList)
        {
            if (seat.UID == id)
                return seat;
        }
        Debug.Log("Could Not Find Seat of ID " + id);
        return null;
    }

}
