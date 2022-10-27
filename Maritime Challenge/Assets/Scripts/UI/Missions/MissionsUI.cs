using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionsUI : MonoBehaviour
{
    [SerializeField]
    private Text MissionTitle;
    [SerializeField]
    private Button ClaimButton;
    public Vector3 ButtonPosition { get { return ClaimButton.transform.position; } }
    [SerializeField]
    private Image MissionProgressFill;
    [SerializeField]
    private Text MissionProgressText;
    [SerializeField]
    private GameObject ClaimedOverlay;

    public int SortOrderRef = 0;

    private Mission mission = null;
    public Mission Mission { get { return mission; } }

    private event Action<MissionsUI> onButtonClickedAction;

    public void Init(Mission mission, int currProg, int reqProg, Action<MissionsUI> action)
    {
        this.mission = mission;

        MissionTitle.text = mission.MissionName;
        MissionProgressFill.fillAmount = (float)currProg / reqProg;
        MissionProgressText.text = (((float)currProg / reqProg) * 100) + "%";

        ClaimButton.gameObject.SetActive(false);


        if (currProg >= reqProg)
        {
            MissionProgressText.gameObject.SetActive(false);
            ClaimButton.gameObject.SetActive(true);
            ClaimButton.onClick.AddListener(OnClaimButtonClicked);
        }
        onButtonClickedAction = action;
    }

    public void SetCompleted()
    {
        ClaimedOverlay.gameObject.SetActive(true);
        ClaimButton.gameObject.SetActive(false);
        MissionProgressText.text = "CLAIMED";
    }
 

    private void OnClaimButtonClicked()
    {
        onButtonClickedAction?.Invoke(this);
    }

}
