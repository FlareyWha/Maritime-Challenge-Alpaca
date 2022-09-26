using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirHockeyMinigame : NetworkBehaviour
{
    [SerializeField]
    private GameObject AirHockeyGamePanel, GameCanvas;

    [SerializeField]
    private GameObject Puck;
    [SerializeField]
    private AirHockeyPaddle[] PlayerPaddle;
    [SerializeField]
    private AirHockeySeat[] PlayerSeats;


    private readonly SyncDictionary<int, Player> playersList = new SyncDictionary<int, Player>();

    private int zoomValue = 3;

    private void Start()
    {
        AirHockeyGamePanel.SetActive(false);
        GameCanvas.SetActive(false);
        Puck.gameObject.SetActive(false);
        for (int i = 0; i < PlayerPaddle.Length; i++)
            PlayerPaddle[i].gameObject.SetActive(false);
    }


    public void PlayerJoinGame(int seatID, Player player)
    {
        EnterTable(seatID);
        // Send to Server
        ServerOnPlayerJoinGame(seatID, PlayerData.MyPlayer);
    }

    [Command(requiresAuthority = false)]
    public void ServerOnPlayerJoinGame(int seatID, Player player)
    {
        AirHockeyGamePanel.SetActive(true); // for server

        // Assign Player Paddle
        if (playersList.ContainsKey(seatID))
            playersList.Remove(seatID);
        playersList.Add(seatID, player);
        PlayerSeats[seatID].AssignPaddleControl(player);

        // Update All Clients
        OnPlayerJoinGameCallback(seatID, player);

        // Start Game
        if (playersList.Count == 2)
            StartGame();
    }

    [ClientRpc]
    private void OnPlayerJoinGameCallback(int seatID, Player player)
    {
        // Take Up Seat
        AirHockeySeat seat = PlayerSeats[seatID];
        if (seat == null)
            return;

        seat.enabled = false;
     
    }

    [Command(requiresAuthority = false)]
    public void ServerOnPlayerLeftGame(Player player)
    {
        int seatID = GetPlayerSeatID(player);
        playersList.Remove(seatID);
        if (playersList.Count == 0)
            AirHockeyGamePanel.SetActive(false);

        PlayerSeats[seatID].RevokePaddleControl();

        OnPlayerLeftGameCallback(seatID, player);
    }

    [ClientRpc]
    private void OnPlayerLeftGameCallback(int seatID, Player player)
    {
        // Free Seat
        AirHockeySeat seat = PlayerSeats[seatID];
        if (seat == null)
            return;

        seat.enabled = true;
    }

    [Client]
    private void EnterTable(int seatID)
    {
        AirHockeySeat seat = PlayerSeats[seatID];
        // Hide Main UI
        UIManager.Instance.ToggleMainUI(false);
        // Show Air Hockey Game GOs
        AirHockeyGamePanel.SetActive(true);
        GameCanvas.SetActive(true);
        Puck.gameObject.SetActive(true);
        for (int i = 0; i < PlayerPaddle.Length; i++)
            PlayerPaddle[i].gameObject.SetActive(true);
        // Camera ANims
        PlayerFollowCamera.Instance.SetFollowTarget(AirHockeyGamePanel.gameObject);
        PlayerFollowCamera.Instance.ZoomCameraInOut(zoomValue, 1.0f);
        if (seat.OnOppositeSide)
            PlayerFollowCamera.Instance.RotateCamera(transform.rotation.eulerAngles.z + 180, 1.0f);
        else
            PlayerFollowCamera.Instance.RotateCamera(transform.rotation.eulerAngles.z, 1.0f);

        // Player Things
        PlayerData.MyPlayer.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
        PlayerData.MyPlayer.transform.position = seat.transform.position;
        PlayerData.MyPlayer.SetOrderInLayer(1);
    }

    [Client]
    private void LeaveTable()
    {
        // Show Main UI
        UIManager.Instance.ToggleMainUI(true);
        // Hide Air Hockey Game GOs
        AirHockeyGamePanel.SetActive(false);
        GameCanvas.SetActive(false);
        Puck.gameObject.SetActive(false);
        for (int i = 0; i < PlayerPaddle.Length; i++)
            PlayerPaddle[i].gameObject.SetActive(false);
        // Reset Camera Anims
        PlayerFollowCamera.Instance.ResetAll(0.7f);
        // Player tings
        PlayerData.MyPlayer.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
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

    public int GetAssignedSeatID(AirHockeySeat seat)
    {
        for (int i = 0; i < PlayerSeats.Length; i++)
        {
            if (PlayerSeats[i] == seat)
                return i;
        }
        Debug.Log("Could not find seat!");
        return -1;
    }
}
