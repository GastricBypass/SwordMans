using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoMainMenuManager : MonoBehaviour
{
    public GameSettingsManager gsm;

    public GameObject mainMenu;
    public Button mainMenuStartOption;

    public GameObject settingsMenu;
    public SettingsMenuManager settingsMenuManager;
    public Button settingsMenuStartOption;

    public Button backButton;

    public AudioClip clickSound;
    public AudioClip selectSound;
    public AudioSource audioSource;
    
    public Text tutorialNumberOfPlayers;
    private int numPlayers = 1;

    public void Start()
    {
        Time.timeScale = 0;

        GameSettingsManager[] gsms = FindObjectsOfType<GameSettingsManager>();

        if (gsms.Length > 1)
        {
            for (int i = 0; i < gsms.Length; i++)
            {
                if (gsms[i].active)
                {
                    gsm = gsms[i];
                }
                else
                {
                    Destroy(gsms[i].gameObject);
                }
            }
        }
        else
        {
            gsm = gsms[0];
            gsm.active = true;
        }

        gsm.inDemo = true;
        numPlayers = gsm.numberOfPlayers;
        tutorialNumberOfPlayers.text = numPlayers.ToString();

        audioSource = this.gameObject.AddComponent<AudioSource>();
        
        settingsMenuManager = this.GetComponent<SettingsMenuManager>();

        UIManager ui = FindObjectOfType<UIManager>();
        ui.paused = true;

        DisableAllMenus();
        mainMenu.SetActive(true);
        backButton.gameObject.SetActive(false);
        mainMenuStartOption.Select();

        colorizeHealthBarsSelected = toggleColorizeHealthBars.transform.Find("Selected").gameObject;
        showHealthValuesSelected = toggleShowHealthValues.transform.Find("Selected").gameObject;
        showBloodSelected = toggleShowBlood.transform.Find("Selected").gameObject;
        musicOnSelected = toggleMusicOn.transform.Find("Selected").gameObject;
        toggleWindowedSelected = toggleWindowed.transform.Find("Selected").gameObject;

        StartCoroutine(RepeatedlyTryToSetSettings(0.1f));

        GoToTab(0);
    }

    public void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            BackButtonPressed();
        }
        if (Input.GetButtonDown("Start"))
        {
            PlayClickSound();
            BackToMainMenu();
        }

        if (settingsMenu.activeSelf)
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

    public void DisableAllMenus()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void PlayTutorialPressed()
    {
        gsm.LoadTutorial();
        Time.timeScale = 1;
    }

    public void SettingsButtonPressed()
    {
        DisableAllMenus();
        settingsMenu.SetActive(true);
        settingsMenuStartOption.Select();
    }

    public void PlayClickSound()
    {
        if (gsm == null)
        {
            return;
        }

        //audioSource.clip = clickSound;
        audioSource.volume = gsm.settings.effectsVolume;
        audioSource.PlayOneShot(clickSound);
    }

    public void PlaySelectSound()
    {
        if (gsm == null)
        {
            return;
        }

        //audioSource.clip = selectSound;
        audioSource.volume = gsm.settings.effectsVolume;
        audioSource.PlayOneShot(selectSound);
    }

    public void IncNumPlayers()
    {
        numPlayers++;
        if (numPlayers > 4)
        {
            numPlayers = 1;
        }

        tutorialNumberOfPlayers.text = numPlayers.ToString();
        gsm.numberOfPlayers = numPlayers;
    }

    public void DecNumPlayers()
    {
        numPlayers--;
        if (numPlayers < 1)
        {
            numPlayers = 4;
        }
        
        tutorialNumberOfPlayers.text = numPlayers.ToString();
        gsm.numberOfPlayers = numPlayers;
    }

    public void BackToMainMenu()
    {
        if (mainMenu.activeSelf)
        {
            return;
        }

        //PlayClickSound();
        DisableAllMenus();
        mainMenu.SetActive(true);
        backButton.gameObject.SetActive(false);
        mainMenuStartOption.Select();
    }

    public void BackButtonPressed()
    {
        BackToMainMenu();
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }
    /************************************************************************************************************************************/
    // From the settings menu manager because it was more confusing than it was worth to make a new settings menu manager for the demo //
    // v     v     v     v     v     v     v     v     v     v     v     v     v     v     v     v     v     v     v     v     v     v //
    /************************************************************************************************************************************/

    public Button toggleColorizeHealthBars;
    private GameObject colorizeHealthBarsSelected;

    public Button toggleShowHealthValues;
    private GameObject showHealthValuesSelected;

    public Button toggleShowBlood;
    private GameObject showBloodSelected;

    public Button toggleWindowed;
    private GameObject toggleWindowedSelected;

    public Slider resolutionSlider;
    public Text resolution;

    public Slider graphicsQualitySlider;
    public Text graphicsQuality;

    public Button toggleMusicOn;
    private GameObject musicOnSelected;

    public Text musicVolume;
    public Slider musicVolumeSlider;
    public Text effectsVolume;
    public Slider effectsVolumeSlider;

    public List<Image> tabs;
    public List<GameObject> tabsOptions;
    public List<Button> tabButtons;
    private int tabIndex = 0;
    private Color selectedColor = new Color(8f / 255f, 20f / 255f, 35f / 255f, 180f / 255f);
    private Color unselectedColor = new Color(50f / 255f, 65f / 255f, 80f / 255f, 180f / 255f);

    public List<Image> weaponSelectButtons;
    public List<GameObject> weaponDescriptions;

    public IEnumerator RepeatedlyTryToSetSettings(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        if (gsm.data == null)
        {
            StartCoroutine(RepeatedlyTryToSetSettings(time));
        }
        else
        {
            audioSource.mute = true; // Changing sliders triggers the onChange for the sliders which makes it make the select sound when the game starts.

            colorizeHealthBarsSelected.SetActive(gsm.settings.colorizeHealthBars);
            showHealthValuesSelected.SetActive(gsm.settings.showHealthValues);
            showBloodSelected.SetActive(gsm.settings.showBlood);

            musicOnSelected.SetActive(gsm.settings.musicOn);
            gsm.SetMusicOn(gsm.settings.musicOn);

            musicVolumeSlider.value = (int)(gsm.settings.musicVolume * 100);
            effectsVolumeSlider.value = (int)(gsm.settings.effectsVolume * 100);
            musicVolume.text = musicVolumeSlider.value.ToString();
            effectsVolume.text = effectsVolumeSlider.value.ToString();

            toggleWindowedSelected.SetActive(gsm.settings.windowed);

            graphicsQualitySlider.maxValue = QualitySettings.names.Length - 1;
            graphicsQualitySlider.value = gsm.settings.graphicsQuality;
            graphicsQuality.text = QualitySettings.names[gsm.settings.graphicsQuality];

            resolutionSlider.maxValue = Screen.resolutions.Length - 1;
            resolutionSlider.value = gsm.settings.resolution;
            resolution.text = Screen.resolutions[gsm.settings.resolution].width + " x " + Screen.resolutions[gsm.settings.resolution].height;

            ShowWeaponDescription(0);

            ApplyGraphicsChanges();

            audioSource.mute = false;
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
        PlayClickSound();
        HideAllTabs();
        tabs[tabNumber].color = selectedColor;
        tabsOptions[tabNumber].SetActive(true);
        tabButtons[tabNumber].Select();
        tabIndex = tabNumber;
    }

    private void HideAllTabs()
    {
        for (int i = 0; i < tabs.Count; i++)
        {
            tabs[i].color = unselectedColor;
            tabsOptions[i].SetActive(false);
        }
    }

    public void ToggleColorizeHealthBars()
    {
        gsm.settings.SetColorizeHealthBars(!gsm.settings.colorizeHealthBars);
        colorizeHealthBarsSelected.SetActive(!colorizeHealthBarsSelected.activeSelf);
    }

    public void ToggleShowHealthValues()
    {
        gsm.settings.SetShowHealthValues(!gsm.settings.showHealthValues);
        showHealthValuesSelected.SetActive(!showHealthValuesSelected.activeSelf);
    }

    public void ToggleShowBlood()
    {
        gsm.settings.SetShowBlood(!gsm.settings.showBlood);
        showBloodSelected.SetActive(!showBloodSelected.activeSelf);
    }

    public void ToggleWindowed()
    {
        gsm.settings.SetWindowed(!gsm.settings.windowed);
        toggleWindowedSelected.SetActive(!toggleWindowedSelected.activeSelf);
    }

    public void SetResolution()
    {
        int resolutionIndex = (int)resolutionSlider.value;

        gsm.settings.SetResolution(resolutionIndex);
        resolution.text = Screen.resolutions[resolutionIndex].width + " x " + Screen.resolutions[resolutionIndex].height;
    }

    public void SetGraphicsQuality()
    {
        int qualityIndex = (int)graphicsQualitySlider.value;

        gsm.settings.SetGraphicsQuality(qualityIndex);
        graphicsQuality.text = QualitySettings.names[qualityIndex];
    }

    public void ApplyGraphicsChanges()
    {
        int resolutionIndex = (int)resolutionSlider.value;
        int qualityIndex = (int)graphicsQualitySlider.value;

        Screen.SetResolution(Screen.resolutions[resolutionIndex].width, Screen.resolutions[resolutionIndex].height, !gsm.settings.windowed);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void ToggleMusicOn()
    {
        gsm.SetMusicOn(!gsm.settings.musicOn);
        musicOnSelected.SetActive(!musicOnSelected.activeSelf);
    }

    public void SetMusicVolume()
    {
        float volume = musicVolumeSlider.value / 100f;
        gsm.SetMusicVolume(volume);

        musicVolume.text = musicVolumeSlider.value.ToString();
    }

    public void SetEffectsVolume()
    {
        float volume = effectsVolumeSlider.value / 100f;
        gsm.settings.SetEffectsVolume(volume);

        effectsVolume.text = effectsVolumeSlider.value.ToString();
    }

    public void ShowWeaponDescription(int index)
    {
        HideAllDescriptions();
        weaponDescriptions[index].SetActive(true);
        weaponSelectButtons[index].color = selectedColor; // kind of confusing, but this is the opposite color from the background.
    }

    private void HideAllDescriptions()
    {
        for (int i = 0; i < weaponDescriptions.Count; i++)
        {
            weaponDescriptions[i].SetActive(false);
            weaponSelectButtons[i].color = unselectedColor; // kind of confusing, but this is the same color as the background.
        }
    }
}
