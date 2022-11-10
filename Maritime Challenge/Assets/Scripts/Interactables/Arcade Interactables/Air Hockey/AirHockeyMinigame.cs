using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirHockeyMinigame : NetworkBehaviour
{
    [SerializeField]
    private int zoomValue = 3;
    [SerializeField]
    private GameObject AirHockeyGamePanel, GameCanvas;

    [SerializeField]
    private AirHockeyPuck Puck;
  //  [SerializeField]
  //  private AirHockeyPaddle[] PlayerPaddle;
    [SerializeField]
    private AirHockeySeat[] PlayerSeats;

    private readonly SyncList<bool> SeatsTaken = new SyncList<bool>();

    private readonly SyncDictionary<int, uint> playersList = new SyncDictionary<int, uint>();
    private readonly SyncDictionary<int, int> scoresList = new SyncDictionary<int, int>();

    private Vector3 startPuckPos;

    [SerializeField]
    private AudioSource cheerAudioSource;

    private void Start()
    {
        AirHockeyGamePanel.SetActive(false);
        GameCanvas.SetActive(false);
        Puck.gameObject.SetActive(false);
        startPuckPos = Puck.transform.position;

        for (int i = 0; i < PlayerSeats.Length; i++)
        {
            if (playersList.ContainsKey(i))
                PlayerSeats[i].UpdateNameDisplay(GetPlayer(playersList[i]).GetUsername());
            else
                PlayerSeats[i].UpdateNameDisplay("Waiting for Player...");

            PlayerSeats[i].SetIsTaken(SeatsTaken[i]);
        }

        SeatsTaken.Callback += UpdateSeatAvailability;
    }

    public override void OnStartServer()
    {
        for (int i = 0; i < PlayerSeats.Length; i++)
        {
            scoresList.Add(i, 0);
            SeatsTaken.Add(false);
        }
        Puck.gameObject.GetComponent<Collider2D>().enabled = false;
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
        playersList.Add(seatID, player.netId);
        PlayerSeats[seatID].AssignPaddleControl(player);
        SeatsTaken[seatID] = true;

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

        seat.UpdateNameDisplay(player.GetUsername());


    }

    [Command(requiresAuthority = false)]
    public void ServerOnPlayerLeftGame(Player player)
    {
        int seatID = GetPlayerSeatID(player);
        playersList.Remove(seatID);
        if (playersList.Count == 1)
            StopGame();

        PlayerSeats[seatID].RevokePaddleControl();
        SeatsTaken[seatID] = false;

        OnPlayerLeftGameCallback(seatID, player);
    }

    private void UpdateSeatAvailability(SyncList<bool>.Operation op, int index, bool oldItem, bool newItem)
    {
        // Free Seat
        AirHockeySeat seat = PlayerSeats[index];
        if (seat == null)
            return;

        seat.SetIsTaken(newItem);

    }

    [ClientRpc]
    private void OnPlayerLeftGameCallback(int seatID, Player player)
    {
        // Free Seat
        AirHockeySeat seat = PlayerSeats[seatID];
        if (seat == null)
            return;

        seat.UpdateNameDisplay("Waiting for Player...");
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
        // Camera ANims
        PlayerFollowCamera.Instance.SetFollowTarget(AirHockeyGamePanel.gameObject);
        PlayerFollowCamera.Instance.ZoomCameraInOut(zoomValue, 1.0f);
        if (seat.OnOppositeSide)
            PlayerFollowCamera.Instance.RotateCamera(transform.rotation.eulerAngles.z + 180, 1.0f);
        else
            PlayerFollowCamera.Instance.RotateCamera(transform.rotation.eulerAngles.z, 1.0f);

        // Player Things
        PlayerData.MyPlayer.transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        PlayerData.MyPlayer.transform.position = seat.transform.position;
    }

    [Client]
    private void LeaveTable()
    {
        // Show Main UI
        UIManager.Instance.ToggleMainUI(true);
        // Hide Air Hockey Game GOs
        AirHockeyGamePanel.SetActive(false);
        GameCanvas.SetActive(false);
        // Reset Camera Anims
        PlayerFollowCamera.Instance.ResetAll(0.7f);
        // Player tings
        PlayerData.MyPlayer.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
    }

    [Server]
    private void StartGame()
    {
        //Puck.gameObject.SetActive(true);
        Puck.gameObject.GetComponent<Collider2D>().enabled = true;
        ResetPuck();
        StartGameCallback();
    }

    [ClientRpc]
    private void StartGameCallback()
    {
        Puck.gameObject.SetActive(true);
    }

    [Server]
    public void StopGame()
    {
        Puck.gameObject.GetComponent<Collider2D>().enabled = false;
        StopGameCallback();
    }

    [ClientRpc]
    private void StopGameCallback()
    {
        Puck.gameObject.SetActive(false);
    }

    public void OnLeaveGameClicked()
    {
        LeaveTable();

        // Send to Server
        ServerOnPlayerLeftGame(PlayerData.MyPlayer);
    }

    [Server]
    public void OnPuckEnteredGoal(int seatID)
    {
        ResetPuck();

        PlayGoalSFX(transform.position);

        int score = scoresList[seatID];
        score++;
        scoresList[seatID] = score;
        UpdateScoreDisplay(seatID, score);
    }

    [ClientRpc]
    private void PlayGoalSFX(Vector3 pos)
    {
        if (PlayerData.activeSubScene != "ArcadeScene")
            return;

        // Play Cheering SFX
        cheerAudioSource.Play();
    }

    [ClientRpc]
    private void UpdateScoreDisplay(int seatID, int score)
    {
        PlayerSeats[seatID].UpdateScoreDisplay(score);
    }

    private void ResetPuck()
    {
        Puck.transform.position = startPuckPos;
        Puck.ForceStop();
    }

    private int GetPlayerSeatID(Player player)
    {
        foreach (KeyValuePair<int, uint> info in playersList)
        {
            if (info.Value == player.netId)
                return info.Key;
        }
        Debug.LogWarning("Player not recorded in list of seated players!");
        return -1;
    }

    public int GetAssignedSeatID(AirHockeySeat seat)
    {
        for (int i = 0; i < PlayerSeats.Length; i++)
        {
            if (PlayerSeats[i] == seat)
                return i;
        }
        Debug.LogWarning("Could not find seat!");
        return -1;
    }

    private Player GetPlayer(uint playerNetID)
    {
        if (NetworkClient.spawned.TryGetValue(playerNetID, out NetworkIdentity identity))
            return identity.gameObject.GetComponent<Player>();
        else
            return null;
    }

  
}
