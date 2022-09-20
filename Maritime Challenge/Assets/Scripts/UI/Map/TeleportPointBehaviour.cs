using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPointBehaviour : MonoBehaviour
{
    [SerializeField]
    private TeleportPanelManager teleportInfoPanel;

    [SerializeField]
    private string teleportPointName, teleportPointDescription;

    private Vector3 teleportCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        teleportCoordinates = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTeleportInfo()
    {
        teleportInfoPanel.UpdateTeleportInfoPanel(teleportPointName, teleportPointDescription, teleportCoordinates);
    }
}
