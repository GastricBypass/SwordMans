using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class IMainMenuManager : MonoBehaviour
{

    public StandaloneInputModule control;
    public GameSettingsManager gsm;

    public GameObject customizationMenu;
    public CustomizationMenuManager customizationMenuManager;
    public Button customizationMenuStartOption;
    public Transform customizationMenuCameraPosition;

    public AudioClip clickSound;
    public AudioClip selectSound;
    public AudioSource audioSource;

    protected Camera menuCamera;

    public virtual void Start()
    {
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

        audioSource = this.gameObject.AddComponent<AudioSource>();
    }

    public virtual void Update()
    {
        if (Input.GetButton("Cancel"))
        {
            BackButtonPressed();
        }
        if (Input.GetButton("Start"))
        {
            BackButtonPressed();
        }
    }

    public void SendCameraToTransform(Transform newTransform)
    {
        menuCamera.transform.position = newTransform.position;
        menuCamera.transform.rotation = newTransform.rotation;
    }

    public virtual void BackButtonPressed()
    {
        // no action
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
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
}

public class MainMenuManager : IMainMenuManager
{
    public GameObject mainMenu;
    public Button mainMenuStartOption;
    public Transform mainMenuCameraPosition;

    public GameObject playMenu;
    public PlayMenuManager playMenuManager;
    public Button playMenuStartOption;
    public Transform playMenuCameraPosition;

    public GameObject settingsMenu;
    public SettingsMenuManager settingsMenuManager;
    public Button settingsMenuStartOption;
    public Transform settingsMenuCameraPosition;

    public GameObject shopMenu;
    public ShopMenuManager shopMenuManager;
    public Scrollbar shopMenuStartOption;
    public Transform shopMenuCameraPosition;

    public Button backButton;

    public float moveThresh;
    public float moveDelayMS;
    public bool shouldRestoreDefaults = true;

	// Use this for initialization
	public override void Start ()
    {
        Time.timeScale = 0;

        base.Start();

        playMenuManager = this.GetComponent<PlayMenuManager>();
        customizationMenuManager = this.GetComponent<CustomizationMenuManager>();
        settingsMenuManager = this.GetComponent<SettingsMenuManager>();
        shopMenuManager = this.GetComponent<ShopMenuManager>();

        UIManager ui = FindObjectOfType<UIManager>();
        ui.paused = true;

        menuCamera = FindObjectOfType<Camera>();

        DisableAllMenus();
        mainMenu.SetActive(true);
        backButton.gameObject.SetActive(false);
        mainMenuStartOption.Select();
        SendCameraToTransform(mainMenuCameraPosition);
    }

    public void DisableAllMenus()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        playMenu.SetActive(false);
        customizationMenu.SetActive(false);
        customizationMenuManager.ResetPlayerSelectors();
        shopMenu.SetActive(false);
        backButton.gameObject.SetActive(true);
    }

    public void PlayMenuButtonPressed()
    {
        DisableAllMenus();
        playMenu.SetActive(true);

        if (playMenuManager.versus)
        {
            playMenuManager.versusButton.Select();
        }
        else
        {
            playMenuManager.coopButton.Select();
        }

        SendCameraToTransform(playMenuCameraPosition);
        playMenuManager.SetStagePresets();
    }

    public void CustomizationButtonPressed()
    {
        DisableAllMenus();
        customizationMenu.SetActive(true);
        SendCameraToTransform(customizationMenuCameraPosition);
        customizationMenuManager.SetCustomizationPresets();
        gsm.SpawnMenuPlayers();
    }

    public void ShopButtonPressed()
    {
        DisableAllMenus();
        shopMenu.SetActive(true);
        shopMenuStartOption.Select();
        SendCameraToTransform(shopMenuCameraPosition);
        shopMenuManager.SetGoldValue();
    }

    public void SettingsButtonPressed()
    {
        DisableAllMenus();
        settingsMenu.SetActive(true);
        settingsMenuStartOption.Select();
        SendCameraToTransform(settingsMenuCameraPosition);
    }

    public override void BackButtonPressed()
    {
        if (mainMenu.activeSelf)
        {
            return;
        }

        //PlayClickSound();
        DisableAllMenus();
        mainMenu.SetActive(true);
        shouldRestoreDefaults = true;
        backButton.gameObject.SetActive(false);
        mainMenuStartOption.Select();
        SendCameraToTransform(mainMenuCameraPosition);
    }
}
