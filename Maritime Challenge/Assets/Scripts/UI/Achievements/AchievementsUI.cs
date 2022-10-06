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
    private Image ProgressFill;
    [SerializeField]
    private Text ProgressText;

    private Achievement achievement;
    private Action<Achievement> onSelectAction;


    private void Awake()
    {
        button.onClick.AddListener(OnAchievementUIClicked);
    }


    public void Init(Achievement achvment, Action<Achievement> action)
    {
        
    }

    private void OnAchievementUIClicked()
    {
        onSelectAction?.Invoke(achievement);
    }




}
