using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsUI : MonoBehaviour
{
    [SerializeField]
    private Button button;

    [SerializeField]
    private Sprite FilledStarSprite;
    [SerializeField]
    private List<Image> StarsImage;

    [SerializeField]
    private Text TitleText, DescText;
    [SerializeField]
    private Image BackgroundImage;
    [SerializeField]
    private Image ProgressFill;
    [SerializeField]
    private Text ProgressText;

    private Achievement achievement;
    private Action<Achievement> onSelectAction;


    private void Awake()
    {
        button.onClick.AddListener(OnAchievementUIClicked);
    }

    public void Init(Achievement achvment, int currProg, int maxProg, Action<Achievement> action)
    {
        achievement = achvment;
        onSelectAction = action;
        UpdateUI(achvment, currProg, maxProg);
    }

    private void UpdateUI(Achievement achvment, int currProg, int maxProg)
    {
        // Set Background+Title
        BackgroundImage.sprite = achvment.AchievementData.BackgroundSprite;

        // Set Achievement Text
        TitleText.text = achvment.AchievementName;
        DescText.text = achvment.AchievementDescription;

        // Set Progress
        bool requirementMet = currProg >= maxProg;
        ProgressFill.fillAmount = (float)currProg / maxProg;
        if (requirementMet)
        {
            ProgressText.text = "COMPLETED!";
        }
        else
        {
            ProgressText.text = currProg + "/" + maxProg;
        }

        button.interactable = requirementMet;

        // Star Icons
        for (int i = 0; i < achvment.AchievementData.Tier - 1; i++)
        {
            StarsImage[i].sprite = FilledStarSprite;
        }
    }


    private void OnAchievementUIClicked()
    {
        onSelectAction?.Invoke(achievement);
        Debug.Log("Achievement Claim Clicked");
    }

}
