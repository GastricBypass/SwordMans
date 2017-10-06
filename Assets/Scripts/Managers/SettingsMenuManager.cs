using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsMenuManager : MonoBehaviour {

    public MainMenuManager manager;

    public List<Image> tabs;
    public List<GameObject> tabsOptions;
    private int tabIndex = 0;
    private Color selectedColor = new Color(8f / 255f, 20f / 255f, 35f / 255f);
    private Color unselectedColor = new Color(50f / 255f, 65f / 255f, 80f / 255f);

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
    
    public Text musicVolume;
    public Slider musicVolumeSlider;
    public Text effectsVolume;
    public Slider effectsVolumeSlider;

    public InputField hatUnlockText;
    public InputField miscUnlockText;
    public InputField versusStageUnlockText;
    public InputField coopStageUnlockText;

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
        musicVolumeSlider.value = (int)(manager.gsm.settings.musicVolume * 100);
        effectsVolumeSlider.value = (int)(manager.gsm.settings.effectsVolume * 100);
        musicVolume.text = musicVolumeSlider.value.ToString();
        effectsVolume.text = effectsVolumeSlider.value.ToString();

        GoToTab(0);
    }
	
	// Update is called once per frame
	void Update () {
        if (manager.settingsMenu.activeSelf)
        {
            if (Input.GetButtonDown("RightBumper"))
            {
                ForwardTab();
            }

            if (Input.GetButtonDown("LeftBumper"))
            {
                BackTab();
            }
        }
    }

    public void ForwardTab()
    {
        int nextTab = tabIndex + 1;
        if (nextTab > tabs.Count - 1)
        {
            nextTab = 0;
        }

        GoToTab(nextTab);
    }

    public void BackTab()
    {
        int nextTab = tabIndex - 1;
        if (nextTab < 0)
        {
            nextTab = tabs.Count - 1;
        }

        GoToTab(nextTab);
    }

    public void GoToTab(int tabNumber)
    {
        hideAllTabs();
        tabs[tabNumber].color = selectedColor;
        tabsOptions[tabNumber].SetActive(true);
        tabIndex = tabNumber;
    }

    private void hideAllTabs()
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].color = unselectedColor;
            tabsOptions[i].SetActive(false);
        }
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

    public void SetMusicVolume()
    {
        float volume = musicVolumeSlider.value / 100f;
        manager.gsm.SetMusicVolume(volume);

        musicVolume.text = musicVolumeSlider.value.ToString();
    }

    public void SetEffectscVolume()
    {
        float volume = effectsVolumeSlider.value / 100f;
        manager.gsm.settings.effectsVolume = volume;

        effectsVolume.text = effectsVolumeSlider.value.ToString();
    }

    public void UnlockHatByName()
    {
        manager.gsm.data.UnlockHat(hatUnlockText.text);
        hatUnlockText.text = "";
    }

    public void UnlockMiscByName()
    {
        manager.gsm.data.UnlockMisc(miscUnlockText.text);
        miscUnlockText.text = "";
    }

    public void UnlockVersusStageByName()
    {
        manager.gsm.data.UnlockVersusStage(versusStageUnlockText.text);
        versusStageUnlockText.text = "";

    }

    public void UnlockCoopStageByName()
    {
        manager.gsm.data.UnlockCoopStage(coopStageUnlockText.text);
        coopStageUnlockText.text = "";
    }

    public void UnlockAllItemsAndStages()
    {
        foreach(string hat in GameConstants.Unlocks.allHats)
        {
            manager.gsm.data.UnlockHat(hat);
        }

        foreach (string misc in GameConstants.Unlocks.allMisc)
        {
            manager.gsm.data.UnlockMisc(misc);
        }

        foreach (string versusStage in GameConstants.Unlocks.allVersusStages)
        {
            manager.gsm.data.UnlockVersusStage(versusStage);
        }

        foreach (string coopStage in GameConstants.Unlocks.allCoopStages)
        {
            manager.gsm.data.UnlockCoopStage(coopStage);
        }
    }
}
