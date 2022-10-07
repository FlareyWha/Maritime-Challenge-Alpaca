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
        // Set Achievement Text
        TitleText.text = achvment.AchievementName;
        DescText.text = achvment.AchievementDescription;

        // Star Icons
        for (int i = 0; i < achvment.AchievementData.Tier - 1; i++)
        {
            StarsImage[i].sprite = FilledStarSprite;
        }
    }

    private void OnAchievementUIClicked()
    {
        onSelectAction?.Invoke(achievement);
    }

}
