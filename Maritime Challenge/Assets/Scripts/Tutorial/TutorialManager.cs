using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviourSingleton<TutorialManager>
{
    private Tutorial[] tutorialList;

    private Tutorial currentTutorial;

    private int tutorialPhase = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActivateNextPhase()
    {
        tutorialPhase++;
        currentTutorial = tutorialList[tutorialPhase];
    }
}
