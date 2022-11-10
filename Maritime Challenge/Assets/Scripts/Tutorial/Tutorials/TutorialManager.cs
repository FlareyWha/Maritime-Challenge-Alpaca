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

    private bool lastTutorial = false;

    private void Start()
    {
        currentTutorial = tutorialList[0];
        currentTutorial.InitTutorial(tutorialHUDManager);
        TutorialPromptManager.Instance.ActivateTutorialPrompt((TUTORIALID)tutorialPhase);
    }

    private void Update()
    {
        currentTutorial.CheckConditionChanges();

        if (currentTutorial.CheckConditionCleared() && !lastTutorial)
            ActivateNextPhase();
    }

    void ActivateNextPhase()
    {
        if (tutorialPhase == tutorialList.Length - 1)
        {
            TutorialPromptManager.Instance.ActivateTutorialPrompt(TUTORIALID.TUTORIAL_COMPLETE);
            lastTutorial = true;
            tutorialHUDManager.UpdateTutorialHUD("You are free", "Explore the area, interact with other guests or register an account", 0, 0);
        }
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
        UIManager.Instance.ToggleJoystick(true);
        PlayerData.CommandsHandler.SwitchSubScene("WorldHubScene", SceneManager.StartWorldHubSpawnPos);
    }
}
