using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportPanelManager : MonoBehaviourSingleton<TeleportPanelManager>
{
    [SerializeField]
    private MapBehaviour map;

    private RectTransform rectTransform;

    [SerializeField]
    private Text teleportPointNameText, teleportPointDescriptionText;

    private Vector3 currentTeleportCoordinates = Vector3.zero;

    private Vector3 stayingCoords = new Vector3(0, 15, 0);

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector3(0, -330, 0);
    }

    public void UpdateTeleportInfoPanel(string teleportPointName, string teleportPointDescription, Vector3 teleportCoordinates)
    {
        if (Vector3.Distance(transform.localPosition, stayingCoords) > 0.2f)
        {
            StartCoroutine(UIManager.ToggleFlyInAnim(rectTransform, new Vector3(0, -330, 0), stayingCoords, 0.5f, null));
        }

        teleportPointNameText.text = teleportPointName;
        teleportPointDescriptionText.text = teleportPointDescription;
        currentTeleportCoordinates = teleportCoordinates;
    }

    public void DeactivateTeleportInfoPanel()
    {
        StartCoroutine(UIManager.ToggleFlyOutAnim(rectTransform, new Vector3(0, -330, 0), stayingCoords, 0.5f, null));
    }

    public void Teleport()
    {
        PlayerData.MyPlayer.gameObject.transform.position = currentTeleportCoordinates;
        gameObject.transform.localPosition = new Vector3(0, -330, 0);

        //Call loading screen or smth
    }
}
