using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenuManager : MonoBehaviour {

    public MainMenuManager manager;

    public bool versus;

    public GameObject playOptionsMenu;

    public Button versusButton;
    public GameObject versusMenu;
    public Button versusMenuStartOption;

    public Button coopButton;
    public GameObject coopMenu;
    public Button coopMenuStartOption;

    public List<string> stages;
    public Text selectedStage;
    public Text numberOfPlayers;
    public Text numberOfAIPlayers;

    public List<string> coopStages;
    public Text coopSelectedStage;
    public Text coopNumberOfPlayers;
    

    private int activeStageIndex;
    private int numPlayers = 1;
    private int numAIPlayers = 0;

    private int coopActiveStageIndex;

    // Use this for initialization
    void Start ()
    {
        versusMenu.SetActive(false);
        coopMenu.SetActive(false);

        numPlayers = manager.gsm.numberOfPlayers;
        numAIPlayers = manager.gsm.numberOfAIPlayers;
        numberOfPlayers.text = manager.gsm.numberOfPlayers.ToString();
        numberOfAIPlayers.text = manager.gsm.numberOfAIPlayers.ToString();
        coopNumberOfPlayers.text = manager.gsm.numberOfPlayers.ToString();

        if (GSMStagesEqual(stages))
        {
            activeStageIndex = manager.gsm.activeStageIndex;
            selectedStage.text = stages[activeStageIndex];
        }
        else if (GSMStagesEqual(coopStages))
        {
            coopActiveStageIndex = manager.gsm.activeStageIndex;
            coopSelectedStage.text = coopStages[coopActiveStageIndex];
        }
        else
        {
            activeStageIndex = 0;
            coopActiveStageIndex = 0;

            selectedStage.text = stages[activeStageIndex];
            coopSelectedStage.text = coopStages[coopActiveStageIndex];
        }
	}
	
	// Update is called once per frame
	void Update () {
		if (manager.shouldRestoreDefaults)
        {
            versusMenu.SetActive(false);
            coopMenu.SetActive(false);
            playOptionsMenu.SetActive(true);
            manager.shouldRestoreDefaults = false;
        }
	}

    public void VersusButtonPressed()
    {
        playOptionsMenu.SetActive(false);
        versusMenu.SetActive(true);
        versusMenuStartOption.Select();
        versus = true;

        if (!GSMStagesEqual(stages))
        {
            manager.gsm.activeStageIndex = 0;
            manager.gsm.SetStages(stages);
        }
        
    }

    public void CoopButtonPressed()
    {
        playOptionsMenu.SetActive(false);
        coopMenu.SetActive(true);
        coopMenuStartOption.Select();
        versus = false;

        if (!GSMStagesEqual(coopStages))
        {
            manager.gsm.activeStageIndex = 0;
            manager.gsm.SetStages(coopStages);
        }
    }
    
    public void PlayButtonPressed()
    {
        string nextStage;

        if (versus)
        {
            nextStage = selectedStage.text;
        }
        else
        {
            nextStage = coopSelectedStage.text;
        }
        manager.gsm.LoadStage(nextStage);
        Time.timeScale = 1;
    }

    public void BackStagePressed()
    {
        if (versus)
        {
            activeStageIndex--;
            if (activeStageIndex < 0)
            {
                activeStageIndex = stages.Count - 1;
            }

            selectedStage.text = stages[activeStageIndex];
            manager.gsm.activeStageIndex = activeStageIndex;
        }

        else
        {
            coopActiveStageIndex--;
            if (coopActiveStageIndex < 0)
            {
                coopActiveStageIndex = coopStages.Count - 1;
            }

            coopSelectedStage.text = coopStages[coopActiveStageIndex];
            manager.gsm.activeStageIndex = coopActiveStageIndex;
        }
    }

    public void ForwardStagePressed()
    {
        if (versus) {
            activeStageIndex++;
            if (activeStageIndex > stages.Count - 1)
            {
                activeStageIndex = 0;
            }
            
            selectedStage.text = stages[activeStageIndex];
            manager.gsm.activeStageIndex = activeStageIndex;
        }

        else
        {
            coopActiveStageIndex++;
            if (coopActiveStageIndex > coopStages.Count - 1)
            {
                coopActiveStageIndex = 0;
            }

            coopSelectedStage.text = coopStages[coopActiveStageIndex];
            manager.gsm.activeStageIndex = coopActiveStageIndex;
        }
    }

    public void IncNumPlayers()
    {
        numPlayers++;
        if (numPlayers > 4)
        {
            numPlayers = 1;
        }

        if (numPlayers + numAIPlayers > 4)
        {
            numAIPlayers = 4 - numPlayers;
            numberOfAIPlayers.text = numAIPlayers.ToString();
            manager.gsm.numberOfAIPlayers = numAIPlayers;
        }

        numberOfPlayers.text = numPlayers.ToString();
        coopNumberOfPlayers.text = numPlayers.ToString();
        manager.gsm.numberOfPlayers = numPlayers;
    }

    public void DecNumPlayers()
    {
        numPlayers--;
        if (numPlayers < 1)
        {
            numPlayers = 4;
        }

        if (numPlayers + numAIPlayers > 4)
        {
            numAIPlayers = 4 - numPlayers;
            numberOfAIPlayers.text = numAIPlayers.ToString();
            manager.gsm.numberOfAIPlayers = numAIPlayers;
        }

        numberOfPlayers.text = numPlayers.ToString();
        coopNumberOfPlayers.text = numPlayers.ToString();
        manager.gsm.numberOfPlayers = numPlayers;
    }

    public void IncNumAIPlayers()
    {
        numAIPlayers++;
        if (numAIPlayers > 4 - numPlayers)
        {
            numAIPlayers = 0;
        }

        numberOfAIPlayers.text = numAIPlayers.ToString();
        manager.gsm.numberOfAIPlayers = numAIPlayers;
    }

    public void DecNumAIPlayers()
    {
        numAIPlayers--;
        if (numAIPlayers < 0)
        {
            numAIPlayers = 4 - numPlayers;
        }

        numberOfAIPlayers.text = numAIPlayers.ToString();
        manager.gsm.numberOfAIPlayers = numAIPlayers;
    }

    private bool GSMStagesEqual(List<string> compare)
    {
        return manager.gsm.stages.Count == compare.Count;
    }
}
