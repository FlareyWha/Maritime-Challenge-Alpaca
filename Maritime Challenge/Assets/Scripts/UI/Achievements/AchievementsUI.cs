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
    private Image BackgroundImage, MaskOverlayImage;
    [SerializeField]
    private Image ProgressFill;
    [SerializeField]
    private Text ProgressText;
    [SerializeField]
    private GameObject CompletedOverlay;

    public int SortOrderRef = 0;

    public Achievement LinkedAchievement;
    private Action<AchievementsUI> onSelectAction;


    private void Awake()
    {
        button.onClick.AddListener(OnAchievementUIClicked);
    }

    public void Init(Achievement achvment, int currProg, int maxProg, Action<AchievementsUI> action)
    {
        LinkedAchievement = achvment;
        onSelectAction = action;
        UpdateUI(achvment, currProg, maxProg);
    }

    public void SetCompleted(Achievement achvment)
    {
        // Set Background+Title
        BackgroundImage.sprite = achvment.AchievementData.BackgroundSprite;
        // Star Icons
        for (int i = 0; i < achvment.AchievementData.Tier - 1; i++)
        {
            StarsImage[i].sprite = FilledStarSprite;
        }
        // Set Achievement Text
        TitleText.text = achvment.AchievementName;
        DescText.text = achvment.AchievementDescription;
        ProgressText.text = "COMPLETED!";
        CompletedOverlay.SetActive(true);

    }
    private void UpdateUI(Achievement achvment, int currProg, int maxProg)
    {
        // Set Background+Title
        BackgroundImage.sprite = achvment.AchievementData.BackgroundSprite;
        MaskOverlayImage.sprite = achvment.AchievementData.BackgroundSprite;

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
        onSelectAction?.Invoke(this);
    }

}
