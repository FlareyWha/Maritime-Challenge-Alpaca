using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirHockeyMinigame : NetworkBehaviour
{

    [SerializeField]
    private GameObject AirHockeyGamePanel;

    [SerializeField]
    private GameObject Puck;
    [SerializeField]
    private AirHockeyPaddle[] PlayerPaddle;

    
    private readonly SyncList<Player> players = new SyncList<Player>();

    private int zoomValue = 5;


    void Awake()
    {
        AirHockeyGamePanel.SetActive(false);
    }



    public void PlayerJoinGame(int seatID, Player player)
    {
        EnterTable(players.Count == 1);
        // Send to Server
        ServerOnPlayerJoinGame(seatID, player);
    }

    [Command]
    private void ServerOnPlayerJoinGame(int seatID, Player player)
    {
     
        // Assign Player Paddle
        players.Add(player);
        PlayerPaddle[players.Count - 1].AssignController(player.connectionToClient);

        // Update All Clients
        ClientOnPlayerJoinGame(seatID);

        // Start Game
        if (players.Count == 2)
            StartGame();
    }

    [ClientRpc]
    private void ClientOnPlayerJoinGame(int seatID)
    {
        // Take Up Seat
        AirHockeySeat seat = AirHockeySeat.GetSeat(seatID);
        if (seat == null)
            return;

        seat.enabled = false;
    }


    [Client]
    private void EnterTable(bool oppSide)
    {
        AirHockeyGamePanel.SetActive(true);

        PlayerFollowCamera.Instance.ZoomCameraInOut(zoomValue, 0.7f);
        if (oppSide)
            PlayerFollowCamera.Instance.FlipCamera(0.5f);
    }

    [Client]
    private void LeaveTable()
    {
        AirHockeyGamePanel.SetActive(false);

        PlayerFollowCamera.Instance.ResetAll(0.7f);
    }

    [Server]
    private void StartGame()
    {

    }

    [Server]
    public void StopGame()
    {

    }

    public void OnLeaveGameClicked()
    {
        LeaveTable();

        // server things
    }
}
