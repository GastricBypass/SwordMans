using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayMenuManager : MonoBehaviour {

    public MainMenuManager manager;

    public List<string> stages;
    public Text selectedStage;
    public Text numberOfPlayers;

    private int activeStageIndex;
    private int numPlayers = 1;

    // Use this for initialization
    void Start ()
    {
        numPlayers = manager.gsm.numberOfPlayers;
        numberOfPlayers.text = manager.gsm.numberOfPlayers.ToString();
        activeStageIndex = manager.gsm.activeStageIndex;
        selectedStage.text = stages[activeStageIndex];

        manager.gsm.SetStages(stages);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayButtonPressed()
    {
        manager.gsm.LoadStage(selectedStage.text);
        Time.timeScale = 1;
    }

    public void BackStagePressed()
    {
        activeStageIndex--;
        if (activeStageIndex < 0)
        {
            activeStageIndex = stages.Count - 1;
        }

        selectedStage.text = stages[activeStageIndex];
        manager.gsm.activeStageIndex = activeStageIndex;
    }

    public void ForwardStagePressed()
    {
        activeStageIndex++;
        if (activeStageIndex > stages.Count - 1)
        {
            activeStageIndex = 0;
        }

        selectedStage.text = stages[activeStageIndex];
        manager.gsm.activeStageIndex = activeStageIndex;
    }

    public void IncNumPlayers()
    {
        numPlayers++;
        if (numPlayers > 4)
        {
            numPlayers = 1;
        }

        numberOfPlayers.text = numPlayers.ToString();
        manager.gsm.numberOfPlayers = numPlayers;
    }

    public void DecNumPlayers()
    {
        numPlayers--;
        if (numPlayers < 1)
        {
            numPlayers = 4;
        }

        numberOfPlayers.text = numPlayers.ToString();
        manager.gsm.numberOfPlayers = numPlayers;
    }
}
