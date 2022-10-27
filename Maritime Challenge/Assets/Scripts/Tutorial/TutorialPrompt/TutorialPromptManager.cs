using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialPromptManager : MonoBehaviourSingleton<TutorialPromptManager>
{
    [SerializeField]
    private GameObject tutorialPrompt;

    [SerializeField]
    private Text tutorialText;

    [SerializeField]
    private Button prevButton, nextButton, doneButton;

    // Start is called before the first frame update
    void Start()
    {
        tutorialText.text = "Tutorial complete! Press done to complete the tutorial.";
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void ActivateTutorialPrompt()
    {
        tutorialPrompt.SetActive(true);
    }

    public void OnDoneButtonPressed()
    {
        PlayerData.CommandsHandler.SwitchSubScene("WorldHubScene", SceneManager.StartWorldHubSpawnPos);
    }
}
