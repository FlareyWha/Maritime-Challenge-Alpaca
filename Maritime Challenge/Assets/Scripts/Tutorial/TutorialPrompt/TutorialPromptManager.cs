using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum TUTORIALID
{
    MOVE_JOYSTICK,
    VIEW_PROFILE,
    TELEPORT,
    INTERACTABLE,
    BOARD_BATTLESHIP,
    DOCK,
    TUTORIAL_COMPLETE,
    NO_TUTORIAL
}

public class TutorialPromptManager : MonoBehaviourSingleton<TutorialPromptManager>
{
    public static bool TutorialActive = false;

    private List<List<TutorialPrompt>> tutorialPrompts = new List<List<TutorialPrompt>>();

    private List<TutorialPrompt> activeTutorialPromptList;
    private int activeTutorialPromptListIncrement;

    [SerializeField]
    private GameObject tutorialPrompt;

    [SerializeField]
    private Text tutorialText;

    [SerializeField]
    private Image tutorialImage;

    [SerializeField]
    private Button prevButton, nextButton, doneButton;

    [SerializeField]
    private TextAsset[] tutorialPromptJSONFiles;

    private TUTORIALID currTutorialID = TUTORIALID.NO_TUTORIAL;

    // Start is called before the first frame update
    void Start()
    {
        tutorialText.text = "Tutorial complete! Press done to complete the tutorial.";

        for (int i = 0; i < tutorialPromptJSONFiles.Length; ++i)
        {
            tutorialPrompts.Add(JSONDeseralizer.DeseralizeTutorialPrompts(tutorialPromptJSONFiles[i]));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ActivateTutorialPrompt(TUTORIALID tutorialID)
    {
        GetTutorialPromptList(tutorialID);
        ResetButtons();
        HandleTutorialPromptIncrement();

        TutorialActive = true;
        currTutorialID = tutorialID;

        tutorialPrompt.SetActive(true);

        UIManager.Instance.ToggleJoystick(false);
    }

    public void DeactiveTutorialPrompt()
    {
        TutorialActive = false;

        if (currTutorialID != TUTORIALID.NO_TUTORIAL)
            currTutorialID = TUTORIALID.NO_TUTORIAL;

        tutorialPrompt.SetActive(false);

        UIManager.Instance.ToggleJoystick(true);
    }

    public void NextPage()
    {
        activeTutorialPromptListIncrement++;
        HandleTutorialPromptIncrement();
    }

    public void PreviousPage()
    {
        activeTutorialPromptListIncrement--;
        HandleTutorialPromptIncrement();
    }

    public void GetTutorialPromptList(TUTORIALID tutorialID)
    {
        //Get the tutorial list to show. Tutorials in a series will have same title, so finding the first prompt's title will suffice
        foreach (List<TutorialPrompt> tutorialPromptList in tutorialPrompts)
        {
            if (tutorialPromptList[0].TutorialID == tutorialID)
            {
                activeTutorialPromptList = tutorialPromptList;
                activeTutorialPromptListIncrement = 0;
                return;
            }
        }
    }

    void HandleTutorialPromptIncrement()
    {
        //In unity editor ltr make sure set listeners for the buttons to increase or decrease increment, and set the text/image accordingly.
        if (activeTutorialPromptListIncrement > 0 && activeTutorialPromptListIncrement < activeTutorialPromptList.Count - 1)
        {
            //Enable both prev button and next button
            prevButton.gameObject.SetActive(true);
            nextButton.gameObject.SetActive(true);
            doneButton.gameObject.SetActive(false);
        }
        else
        {
            if (activeTutorialPromptListIncrement == 0)
            {
                //Disable previous button
                prevButton.gameObject.SetActive(false);
                nextButton.gameObject.SetActive(true);
                doneButton.gameObject.SetActive(false);
            }
            if (activeTutorialPromptListIncrement == activeTutorialPromptList.Count - 1)
            {
                //Change next button to done button
                prevButton.gameObject.SetActive(true);
                nextButton.gameObject.SetActive(false);
                doneButton.gameObject.SetActive(true);
            }
        }

        SetTutorialPromptInfo();
    }

    void SetTutorialPromptInfo()
    {
        TutorialPrompt activeTutorialPrompt = activeTutorialPromptList[activeTutorialPromptListIncrement];
        tutorialText.text = activeTutorialPrompt.Description;

        if (activeTutorialPrompt.ImageFilePath == "TutorialPrompts/Images/")
        {
            tutorialImage.gameObject.SetActive(false);
        }
        else
        {
            tutorialImage.gameObject.SetActive(true);
            tutorialImage.sprite = Resources.Load<Sprite>(activeTutorialPrompt.ImageFilePath);
            tutorialImage.preserveAspect = true;
        }
    }

    void ResetButtons()
    {
        prevButton.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(true);
        doneButton.gameObject.SetActive(false);
    }

    public void OnDoneButtonPressed()
    {
        if (currTutorialID == TUTORIALID.TUTORIAL_COMPLETE)
        {
            PlayerData.CommandsHandler.SwitchSubScene("WorldHubScene", SceneManager.StartWorldHubSpawnPos);
        }
        else
            DeactiveTutorialPrompt();
    }
}
