using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Sit : BaseInteractable
{
    public static List<Sit> Sits = new List<Sit>();

    public int SitID;


    private Player playerSeated = null;
    public Player PlayerSeated
    {
        get { return playerSeated; }
        set { playerSeated = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        UpdateInteractMessage();

        SitID = Sits.Count;
        Sits.Add(this);
    }


    public override void Interact()
    {
        if (playerSeated == null)
        {
            Player player = PlayerData.MyPlayer;

            //Set player pos to the seat's pos
            player.transform.position = transform.parent.position;

            //Disable joystick
            UIManager.Instance.ToggleJoystick(false);
            
            //Prob clamp the player pos to the seat or smth and prevent movment
            PlayerData.CommandsHandler.SendPlayerSeatedEvent(SitID, player);
        }
        else if (playerSeated == PlayerData.MyPlayer)
        {
            //Enable joystick
            UIManager.Instance.ToggleJoystick(true);

            //Do the opposite of above
            PlayerData.CommandsHandler.SendPlayerSeatedEvent(SitID, null);
        }

        UpdateInteractMessage();
    }

    public void UpdateInteractMessage()
    {
        if (playerSeated == null)
            interactMessage = "Sit Down?";
        else if (playerSeated == PlayerData.MyPlayer)
            interactMessage = "Stand Up?";

        UIManager.Instance.SetInteractButtonMessage(interactMessage);
    }
}
