using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPointBehaviour : MonoBehaviour
{
    private TeleportManager teleportManager;

    [SerializeField]
    private string teleportPointName, teleportPointDescription;

    private Vector3 teleportCoordinates;

    // Start is called before the first frame update
    void Start()
    {
        teleportManager = TeleportManager.Instance;
        teleportCoordinates = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowTeleportInfo()
    {
        teleportManager.UpdateTeleportInfoPanel(teleportPointName, teleportPointDescription, teleportCoordinates);
    }
}
