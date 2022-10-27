using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviourSingleton<TutorialManager>
{
    [SerializeField]
    private Tutorial[] tutorialList;

    private Tutorial currentTutorial;

    private int tutorialPhase = 0;

    private void Start()
    {
        currentTutorial = tutorialList[0];
        currentTutorial.InitTutorial();
    }

    private void Update()
    {
        currentTutorial.CheckConditionChanges();

        if (currentTutorial.CheckConditionCleared())
            ActivateNextPhase();
    }

    void ActivateNextPhase()
    {
        tutorialPhase++;
        currentTutorial = tutorialList[tutorialPhase];
        currentTutorial.InitTutorial();

        Debug.Log("Current phase: " + tutorialPhase);
    }

    public void SkipTutorial()
    {
        PlayerData.CommandsHandler.SwitchSubScene("WorldHubScene", SceneManager.StartWorldHubSpawnPos);
    }
}
