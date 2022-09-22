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


    private readonly SyncDictionary<int, Player> playersList = new SyncDictionary<int, Player>();

    private int zoomValue = 5;




    public void PlayerJoinGame(int seatID, Player player)
    {
        EnterTable(playersList.Count == 1);
        // Send to Server
        ServerOnPlayerJoinGame(seatID, PlayerData.MyPlayer);
    }

    [Command(requiresAuthority = false)]
    public void ServerOnPlayerJoinGame(int seatID, Player player)
    {

        // Assign Player Paddle
        playersList.Add(seatID, player);
        // AirHockeySeat.GetSeat(seatID).AssignPaddleControl(player);
        PlayerPaddle[0].netIdentity.AssignClientAuthority(player.connectionToClient);

        // Update All Clients
        OnPlayerJoinGameCallback(seatID);

        // Start Game
        if (playersList.Count == 2)
            StartGame();
    }

    [ClientRpc]
    private void OnPlayerJoinGameCallback(int seatID)
    {
        // Take Up Seat
        AirHockeySeat seat = AirHockeySeat.GetSeat(seatID);
        if (seat == null)
            return;

        seat.enabled = false;
    }

    [Command(requiresAuthority = false)]
    public void ServerOnPlayerLeftGame(Player player)
    {
        int seatID = GetPlayerSeatID(player);
        playersList.Remove(seatID);

        AirHockeySeat.GetSeat(seatID).RevokePaddleControl();

        OnPlayerLeftGameCallback(seatID);
    }

    [ClientRpc]
    private void OnPlayerLeftGameCallback(int seatID)
    {
        AirHockeySeat seat = AirHockeySeat.GetSeat(seatID);
        if (seat == null)
            return;

        seat.enabled = true;
    }

    [Client]
    private void EnterTable(bool oppSide)
    {
        // Hide Main UI
        UIManager.Instance.ToggleMainUI(false);
        // Show Air Hockey Game GOs
        AirHockeyGamePanel.SetActive(true);
        Puck.gameObject.SetActive(true);
        for (int i = 0; i < PlayerPaddle.Length; i++)
            PlayerPaddle[i].gameObject.SetActive(true);
        // Camera ANims
        PlayerFollowCamera.Instance.SetFollowTarget(this.gameObject);
        PlayerFollowCamera.Instance.ZoomCameraInOut(zoomValue, 0.7f);
        if (oppSide)
            PlayerFollowCamera.Instance.FlipCamera(0.5f);
    }

    [Client]
    private void LeaveTable()
    {
        // Show Main UI
        UIManager.Instance.ToggleMainUI(true);
        // Hide Air Hockey Game GOs
        AirHockeyGamePanel.SetActive(false);
        Puck.gameObject.SetActive(false);
        for (int i = 0; i < PlayerPaddle.Length; i++)
            PlayerPaddle[i].gameObject.SetActive(false);
        // Reset Camera Anims
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

        // Send to Server
        ServerOnPlayerLeftGame(PlayerData.MyPlayer);
    }

    private int GetPlayerSeatID(Player player)
    {
        foreach (KeyValuePair<int, Player> info in playersList)
        {
            if (info.Value == player)
                return info.Key;
        }
        Debug.LogError("Player not recorded in list of seated players!");
        return -1;
    }
}
