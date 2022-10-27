using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewProfileTutorial : Tutorial
{
    private TutorialNPC tutorialNPC;

    public override void InitTutorial()
    {
        tutorialNPC = GameObject.Find("TutorialNPC").GetComponent<TutorialNPC>();
    }

    public override void CheckConditionChanges()
    {
        if (tutorialNPC.Interacted)
            IncreaseCondition();
    }
}
