using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class Sit : BaseInteractable
{
    public static List<Sit> Sits = new List<Sit>();

    public int SitID;

    [SerializeField]
    private GameObject coffeeCup;

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

        coffeeCup.SetActive(false);
    }

    public override void Interact()
    {
        if (playerSeated == null && CafeManager.Instance.HasDrink)
        {
            Player player = PlayerData.MyPlayer;

            //Set player pos to the seat's pos
            player.transform.position = transform.parent.position;

            //Disable joystick
            UIManager.Instance.ToggleJoystick(false);
            
            //Prob clamp the player pos to the seat or smth and prevent movment
            PlayerData.CommandsHandler.SendPlayerSeatedEvent(SitID, player);

            coffeeCup.SetActive(true);

            CafeManager.Instance.HasDrink = false;
        }
        else if (playerSeated == PlayerData.MyPlayer)
        {
            //Enable joystick
            UIManager.Instance.ToggleJoystick(true);

            //Do the opposite of above
            PlayerData.CommandsHandler.SendPlayerSeatedEvent(SitID, null);

            coffeeCup.SetActive(false);
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
