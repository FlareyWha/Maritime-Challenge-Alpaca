using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableTutorial : Tutorial
{
    private TutorialSign tutorialSign;

    public override void InitTutorial()
    {
        tutorialSign = GameObject.Find("TutotialSign").GetComponent<TutorialSign>();
    }

    public override void CheckConditionChanges()
    {
        if (tutorialSign.Interacted)
            IncreaseCondition();
    }
}
