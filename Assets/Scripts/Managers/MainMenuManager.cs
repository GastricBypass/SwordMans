﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenuManager : MonoBehaviour {

    public StandaloneInputModule control;
    public GameSettingsManager gsm;

    public GameObject mainMenu;
    public Button mainMenuStartOption;
    public Transform mainMenuCameraPosition;
    public GameObject playMenu;
    public Button playMenuStartOption;
    public Transform playMenuCameraPosition;
    public GameObject customizationMenu;
    public Button customizationMenuStartOption;
    public Transform customizationMenuCameraPosition;
    public GameObject settingsMenu;
    public Button settingsMenuStartOption;
    public Transform settingsMenuCameraPosition;

    public float moveThresh;
    public float moveDelayMS;
    private int optionIndex;
    private bool interactable = true;
    public bool shouldRestoreDefaults = true;

    private Camera menuCamera;

	// Use this for initialization
	void Start ()
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

        UIManager ui = FindObjectOfType<UIManager>();
        Time.timeScale = 0;
        ui.paused = true;

        menuCamera = FindObjectOfType<Camera>();
        DisableAllMenus();
        mainMenu.SetActive(true);
        mainMenuStartOption.Select();
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetButton("Cancel"))
        {
            BackButtonPressed();
        }
    }

    public void SendCameraToTransform(Transform newTransform)
    {
        menuCamera.transform.position = newTransform.position;
        menuCamera.transform.rotation = newTransform.rotation;
    } 

    public void DisableAllMenus()
    {
        mainMenu.SetActive(false);
        settingsMenu.SetActive(false);
        playMenu.SetActive(false);
        customizationMenu.SetActive(false);
    }

    public void PlayMenuButtonPressed()
    {
        DisableAllMenus();
        playMenu.SetActive(true);
        playMenuStartOption.Select();
        SendCameraToTransform(playMenuCameraPosition);
    }

    public void CustomizationButtonPressed()
    {
        DisableAllMenus();
        customizationMenu.SetActive(true);
        customizationMenuStartOption.Select();
        SendCameraToTransform(customizationMenuCameraPosition);
        gsm.SpawnMenuPlayers();
    }

    public void SettingsButtonPressed()
    {
        DisableAllMenus();
        settingsMenu.SetActive(true);
        settingsMenuStartOption.Select();
        SendCameraToTransform(settingsMenuCameraPosition);
    }

    public void ExitButtonPressed()
    {
        Application.Quit();
    }

    public void BackButtonPressed()
    {
        DisableAllMenus();
        mainMenu.SetActive(true);
        shouldRestoreDefaults = true;
        mainMenuStartOption.Select();
        SendCameraToTransform(mainMenuCameraPosition);
    }
}
