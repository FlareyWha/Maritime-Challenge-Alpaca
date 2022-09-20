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

    private Vector3 hiddenCoords = new Vector3(0, -330, 0);
    private Vector3 stayingCoords = new Vector3(0, 15, 0);

    [SerializeField]
    private List<TeleportPointBehaviour> teleportPoints = new List<TeleportPointBehaviour>();

    private void Start()
    {
        rectTransform.anchoredPosition = new Vector3(0, -330, 0);

        AddTeleportPoints();
    }

    public void UpdateTeleportInfoPanel(string teleportPointName, string teleportPointDescription, Vector3 teleportCoordinates)
    {
        if (Vector3.Distance(rectTransform.anchoredPosition, hiddenCoords) < 0.2f)
        {
            StartCoroutine(UIManager.ToggleFlyInAnim(rectTransform, hiddenCoords, stayingCoords, 0.5f, null));
        }

        teleportPointNameText.text = teleportPointName;
        teleportPointDescriptionText.text = teleportPointDescription;
        currentTeleportCoordinates = teleportCoordinates;
    }

    public void DeactivateTeleportInfoPanel()
    {
        StartCoroutine(UIManager.ToggleFlyOutAnim(rectTransform, stayingCoords, hiddenCoords, 0.5f, null));
    }

    public void Teleport()
    {
        PlayerData.MyPlayer.gameObject.transform.position = currentTeleportCoordinates;
        gameObject.transform.localPosition = hiddenCoords;

        //Call loading screen or smth
    }

    public TeleportPointBehaviour FindClosestTeleportPoint(Vector3 worldTouchPoint)
    {
        int closestTeleportPointIndex = 0;
        float distance = float.MaxValue;

        for (int i = 0; i < teleportPoints.Count; ++i)
        {
            float currentDistance = Vector3.Distance(worldTouchPoint, teleportPoints[i].transform.position);
            if (currentDistance < distance)
            {
                closestTeleportPointIndex = i;
                distance = currentDistance;
            }
        }

        return teleportPoints[closestTeleportPointIndex];
    }

    public void AddTeleportPoints()
    {
        teleportPoints.Clear();
        teleportPoints.AddRange(FindObjectsOfType<TeleportPointBehaviour>());
    }
}
