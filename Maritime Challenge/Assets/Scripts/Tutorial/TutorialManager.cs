using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviourSingleton<TutorialManager>
{
    private Tutorial[] tutorialList;

    private int tutorialPhase = 0;

    public void LoadTutorial()
    {
        StartCoroutine(TutorialLoading());
    }

    IEnumerator TutorialLoading()
    {
        while (PlayerData.CommandsHandler == null)
            yield return null;

        PlayerData.CommandsHandler.SwitchSubScene("TutorialScene", Vector2.zero);
    }



    void ActivateNextPhase()
    {
        tutorialPhase++;
    }
}
