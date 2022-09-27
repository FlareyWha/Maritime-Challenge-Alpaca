using System;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class AirHockeyGoal : MonoBehaviour
{
    [SerializeField]
    private AirHockeyMinigame gameManager;

    private int seatID;
    public Action<int> OnPuckEntered;


    private void Start()
    {
        OnPuckEntered += gameManager.OnPuckEnteredGoal;
    }

    public void SetSeatID(int id)
    {
        seatID = id;
    }

    [ServerCallback]

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("AirHockeyPuck"))
        {
            Debug.Log("Puck Entered Goal at seat ID: " + seatID);
            OnPuckEntered?.Invoke(seatID);
        }
    }
}
