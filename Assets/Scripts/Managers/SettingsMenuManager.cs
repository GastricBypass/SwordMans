using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenuManager : MonoBehaviour {

    public MainMenuManager manager;

    public Button toggleColorizeHealthBars;
    private GameObject colorizeHealthBarsSelected;

    public Button toggleShowHealthValues;
    private GameObject showHealthValuesSelected;

    public int maxNumberOfRounds;
    public Text numberOfRounds;

    public Button toggleRandomStageSelect;
    private GameObject randomStageSelectSelected;

    public Button toggleMusicOn;
    private GameObject musicOnSelected;

    // Use this for initialization
    void Start ()
    {
        colorizeHealthBarsSelected = toggleColorizeHealthBars.transform.Find("Selected").gameObject;
        showHealthValuesSelected = toggleShowHealthValues.transform.Find("Selected").gameObject;
        randomStageSelectSelected = toggleRandomStageSelect.transform.Find("Selected").gameObject;
        musicOnSelected = toggleMusicOn.transform.Find("Selected").gameObject;

        colorizeHealthBarsSelected.SetActive(manager.gsm.settings.colorizeHealthBars);
        showHealthValuesSelected.SetActive(manager.gsm.settings.showHealthValues);
        numberOfRounds.text = manager.gsm.settings.roundsPerStage.ToString();
        randomStageSelectSelected.SetActive(manager.gsm.settings.randomStateSelect);
        musicOnSelected.SetActive(manager.gsm.settings.musicOn);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToggleColorizeHealthBars()
    {
        manager.gsm.settings.colorizeHealthBars = !manager.gsm.settings.colorizeHealthBars;
        colorizeHealthBarsSelected.SetActive(!colorizeHealthBarsSelected.activeSelf);
    }

    public void ToggleShowHealthValues()
    {
        manager.gsm.settings.showHealthValues = !manager.gsm.settings.showHealthValues;
        showHealthValuesSelected.SetActive(!showHealthValuesSelected.activeSelf);
    }

    public void ForwardNumRounds()
    {
        manager.gsm.settings.roundsPerStage ++;
        
        if (manager.gsm.settings.roundsPerStage > maxNumberOfRounds)
        {
            manager.gsm.settings.roundsPerStage = 1;
        }

        numberOfRounds.text = manager.gsm.settings.roundsPerStage.ToString();
    }

    public void BackNumRounds()
    {
        manager.gsm.settings.roundsPerStage --;

        if (manager.gsm.settings.roundsPerStage < 1)
        {
            manager.gsm.settings.roundsPerStage = maxNumberOfRounds;
        }

        numberOfRounds.text = manager.gsm.settings.roundsPerStage.ToString();
    }

    public void ToggleRandomStageSelect()
    {
        manager.gsm.settings.randomStateSelect = !manager.gsm.settings.randomStateSelect;
        randomStageSelectSelected.SetActive(!randomStageSelectSelected.activeSelf);
    }

    public void ToggleMusicOn()
    {
        manager.gsm.ToggleMusicOn();
        musicOnSelected.SetActive(!musicOnSelected.activeSelf);
    }
}
