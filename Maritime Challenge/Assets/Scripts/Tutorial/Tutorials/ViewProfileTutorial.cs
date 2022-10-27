using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewProfileTutorial : Tutorial
{
    [SerializeField]
    private TutorialNPC tutorialNPC;

    protected override void Start()
    {
        base.Start();
        conditionText = "View the NPC'S Profile";
    }

    public override void CheckConditionChanges()
    {
        if (tutorialNPC.Interacted)
            IncreaseCondition();
    }
}
