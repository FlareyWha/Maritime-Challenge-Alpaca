using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviourSingleton<TutorialManager>
{
    [SerializeField]
    private Tutorial[] tutorialList;

    private Tutorial currentTutorial;

    private int tutorialPhase = 0;

    [SerializeField]
    private TutorialHUDManager tutorialHUDManager;

    private void Start()
    {
        currentTutorial = tutorialList[0];
        currentTutorial.InitTutorial(tutorialHUDManager);
        TutorialPromptManager.Instance.ActivateTutorialPrompt((TUTORIALID)tutorialPhase);
    }

    private void Update()
    {
        currentTutorial.CheckConditionChanges();

        if (currentTutorial.CheckConditionCleared())
            ActivateNextPhase();
    }

    void ActivateNextPhase()
    {
        if (tutorialPhase == tutorialList.Length - 1)
            TutorialPromptManager.Instance.ActivateTutorialPrompt(TUTORIALID.TUTORIAL_COMPLETE);
        else
        {
            tutorialPhase++;
            currentTutorial = tutorialList[tutorialPhase];
            currentTutorial.InitTutorial(tutorialHUDManager);
            TutorialPromptManager.Instance.ActivateTutorialPrompt((TUTORIALID)tutorialPhase);
        }

        Debug.Log("Current phase: " + tutorialPhase);
    }

    public void SkipTutorial()
    {
        PlayerData.CommandsHandler.SwitchSubScene("WorldHubScene", SceneManager.StartWorldHubSpawnPos);
    }
}