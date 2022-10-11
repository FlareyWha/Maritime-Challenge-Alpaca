using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TeleportManager : MonoBehaviourSingleton<TeleportManager>
{
    [SerializeField]
    private MapBehaviour map;

    [SerializeField]
    private RectTransform rectTransform;

    [SerializeField]
    private Text teleportPointNameText, teleportPointDescriptionText;

    private Vector3 currentTeleportCoordinates = Vector3.zero;

    private Vector3 hiddenCoords = new Vector3(0, -390, 0);
    private Vector3 stayingCoords = new Vector3(0, 0, 0);

    [SerializeField]
    private Button backButton;

    private void Start()
    {
        rectTransform.anchoredPosition = hiddenCoords;
    }

    public void UpdateTeleportInfoPanel(string teleportPointName, string teleportPointDescription, Vector3 teleportCoordinates)
    {
        Debug.Log(Vector3.Distance(rectTransform.anchoredPosition, hiddenCoords));

        if (Vector3.Distance(rectTransform.anchoredPosition, hiddenCoords) < 5f)
        {
            StartCoroutine(UIManager.ToggleFlyInAnim(rectTransform, hiddenCoords, stayingCoords, 0.5f, backButton));
        }

        teleportPointNameText.text = teleportPointName;
        teleportPointDescriptionText.text = teleportPointDescription;
        currentTeleportCoordinates = teleportCoordinates;
    }

    public void DeactivateTeleportInfoPanel()
    {
        StartCoroutine(UIManager.ToggleFlyOutAnim(rectTransform, stayingCoords, hiddenCoords, 0.5f, backButton));
    }

    public void Teleport()
    {
        PlayerData.MyPlayer.gameObject.transform.position = currentTeleportCoordinates;
        rectTransform.anchoredPosition = hiddenCoords;

        //Call loading screen or smth
    }
}
