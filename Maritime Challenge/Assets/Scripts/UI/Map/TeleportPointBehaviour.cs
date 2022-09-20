using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPointBehaviour : MonoBehaviour
{
    private TeleportPanelManager teleportPanelManager;

    [SerializeField]
    private string teleportPointName, teleportPointDescription;

    private Vector3 teleportCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        teleportPanelManager = TeleportPanelManager.Instance;
        teleportCoordinates = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTeleportInfo()
    {
        teleportPanelManager.UpdateTeleportInfoPanel(teleportPointName, teleportPointDescription, teleportCoordinates);
    }
}
