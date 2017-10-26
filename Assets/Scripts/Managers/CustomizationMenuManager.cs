﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CustomizationMenuManager : MonoBehaviour {

    public MainMenuManager manager;

    public List<string> colors;
    public List<string> weapons;
    public List<string> hats;
    public List<string> misc;
    public List<string> skins;

    private int[] colorIndeces = new int[] { 0, 1, 2, 3 };
    private int[] weaponIndeces = new int[] { 0, 0, 0, 0 };
    private int[] hatIndeces = new int[] { 0, 0, 0, 0 };
    private int[] miscIndeces = new int[] { 0, 0, 0, 0 };
    private int[] skinIndeces = new int[] { 0, 0, 0, 0 };

    public List<Text> selectedColors;
    public List<Text> selectedWeapons;
    public List<Text> selectedHats;
    public List<Text> selectedMisc;
    public List<Text> selectedSkins;

    public float selectMoveWaitTime = 0.2f;
    public List<Image> playerSelectedImages;
    public List<Button> playerSelectedStartingOptions;
    private List<Button> playersSelected = new List<Button>(new Button[4]);
    public List<bool> playerSelectsCanMove = new List<bool>(new bool[] { true, true, true, true });

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(RepeatedlyTryToSetUnlockables(0.1f));

        for (int i = 0; i < playersSelected.Count; i++)
        {
            playersSelected[i] = playerSelectedStartingOptions[i];
        }

        SetAllIconColors();
    }

    public IEnumerator RepeatedlyTryToSetUnlockables(float time)
    {
        yield return new WaitForSecondsRealtime(time);

        if (manager.gsm.data == null)
        {
            StartCoroutine(RepeatedlyTryToSetUnlockables(time));
        }
        else
        {
            hats = manager.gsm.data.hats;
            misc = manager.gsm.data.misc;
            SetCustomizationPresets();
        }
    }

    public void SetCustomizationPresets()
    {
        for (int i = 0; i < 4; i++)
        {
            selectedColors[i].text = manager.gsm.playerColors[i];
            selectedWeapons[i].text = manager.gsm.playerWeapons[i];
            selectedHats[i].text = manager.gsm.playerHats[i];
            selectedMisc[i].text = manager.gsm.playerMisc[i];
            selectedSkins[i].text = manager.gsm.playerSkins[i];
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (manager.customizationMenu.activeSelf)
        {
            // This could be better... Maybe just iterate through players.
            if (Mathf.Abs(Input.GetAxis("1Horizontal")) > 0.1 || Mathf.Abs(Input.GetAxis("1Vertical")) > 0.1)
            {
                MovePlayerSelection(Input.GetAxis("1Horizontal"), Input.GetAxis("1Vertical"), 1);
            }
            if (Mathf.Abs(Input.GetAxis("2Horizontal")) > 0.1 || Mathf.Abs(Input.GetAxis("2Vertical")) > 0.1)
            {
                MovePlayerSelection(Input.GetAxis("2Horizontal"), Input.GetAxis("2Vertical"), 2);
            }
            if (Mathf.Abs(Input.GetAxis("3Horizontal")) > 0.1 || Mathf.Abs(Input.GetAxis("3Vertical")) > 0.1)
            {
                MovePlayerSelection(Input.GetAxis("3Horizontal"), Input.GetAxis("3Vertical"), 3);
            }
            if (Mathf.Abs(Input.GetAxis("4Horizontal")) > 0.1 || Mathf.Abs(Input.GetAxis("4Vertical")) > 0.1)
            {
                MovePlayerSelection(Input.GetAxis("4Horizontal"), Input.GetAxis("4Vertical"), 4);
            }

            if (Input.GetButtonDown("1Rise"))
            {
                SubmitForPlayerSelected(1);
            }
            if (Input.GetButtonDown("2Rise"))
            {
                SubmitForPlayerSelected(2);
            }
            if (Input.GetButtonDown("3Rise"))
            {
                SubmitForPlayerSelected(3);
            }
            if (Input.GetButtonDown("4Rise"))
            {
                SubmitForPlayerSelected(4);
            }
        }
    }

    public void ForwardColors(int playerNum)
    {
        colorIndeces[playerNum - 1]++;

        if (colorIndeces[playerNum - 1] > colors.Count - 1)
        {
            colorIndeces[playerNum - 1] = 0;
        }

        SetColor(colorIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    public void BackColors(int playerNum)
    {
        colorIndeces[playerNum - 1]--;

        if (colorIndeces[playerNum - 1] < 0)
        {
            colorIndeces[playerNum - 1] = colors.Count - 1;
        }

        SetColor(colorIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    public void ForwardWeapons(int playerNum)
    {
        weaponIndeces[playerNum - 1]++;

        if (weaponIndeces[playerNum - 1] > weapons.Count - 1)
        {
            weaponIndeces[playerNum - 1] = 0;
        }

        SetWeapon(weaponIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    public void BackWeapons(int playerNum)
    {
        weaponIndeces[playerNum - 1]--;

        if (weaponIndeces[playerNum - 1] < 0)
        {
            weaponIndeces[playerNum - 1] = weapons.Count - 1;
        }

        SetWeapon(weaponIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    public void ForwardHats(int playerNum)
    {
        hatIndeces[playerNum - 1]++;

        if (hatIndeces[playerNum - 1] > hats.Count - 1)
        {
            hatIndeces[playerNum - 1] = 0;
        }

        SetHat(hatIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    public void BackHats(int playerNum)
    {
        hatIndeces[playerNum - 1]--;

        if (hatIndeces[playerNum - 1] < 0)
        {
            hatIndeces[playerNum - 1] = hats.Count - 1;
        }

        SetHat(hatIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    public void ForwardMisc(int playerNum)
    {
        miscIndeces[playerNum - 1]++;

        if (miscIndeces[playerNum - 1] > misc.Count - 1)
        {
            miscIndeces[playerNum - 1] = 0;
        }

        SetMisc(miscIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    public void BackMisc(int playerNum)
    {
        miscIndeces[playerNum - 1]--;

        if (miscIndeces[playerNum - 1] < 0)
        {
            miscIndeces[playerNum - 1] = misc.Count - 1;
        }

        SetMisc(miscIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    public void ForwardSkins(int playerNum)
    {
        skinIndeces[playerNum - 1]++;

        if (skinIndeces[playerNum - 1] > skins.Count - 1)
        {
            skinIndeces[playerNum - 1] = 0;
        }

        SetSkin(skinIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    public void BackSkins(int playerNum)
    {
        skinIndeces[playerNum - 1]--;

        if (skinIndeces[playerNum - 1] < 0)
        {
            skinIndeces[playerNum - 1] = skins.Count - 1;
        }

        SetSkin(skinIndeces[playerNum - 1], playerNum);
        manager.gsm.SpawnMenuPlayers();
    }

    private void SetColor(int index, int playerNum)
    {
        selectedColors[playerNum - 1].text = colors[index];
        manager.gsm.playerColors[playerNum - 1] = selectedColors[playerNum - 1].text;
        SetAllIconColors();
    }

    private void SetWeapon(int index, int playerNum)
    {
        selectedWeapons[playerNum - 1].text = weapons[index];
        manager.gsm.playerWeapons[playerNum - 1] = selectedWeapons[playerNum - 1].text;
    }

    private void SetHat(int index, int playerNum)
    {
        selectedHats[playerNum - 1].text = hats[index];
        manager.gsm.playerHats[playerNum - 1] = selectedHats[playerNum - 1].text;
    }

    private void SetMisc(int index, int playerNum)
    {
        selectedMisc[playerNum - 1].text = misc[index];
        manager.gsm.playerMisc[playerNum - 1] = selectedMisc[playerNum - 1].text;
    }

    private void SetSkin(int index, int playerNum)
    {
        selectedSkins[playerNum - 1].text = skins[index];
        manager.gsm.playerSkins[playerNum - 1] = selectedSkins[playerNum - 1].text;
    }

    public void MovePlayerSelection(float xMove, float yMove, int playerNumber)
    {
        if (Mathf.Abs(xMove) > Mathf.Abs(yMove))
        {
            if (xMove > 0)
            {
                MovePlayerSelectionByDirection("right", playerNumber);
            }
            else
            {
                MovePlayerSelectionByDirection("left", playerNumber);
            }
        }

        else
        {
            if (yMove > 0)
            {
                MovePlayerSelectionByDirection("down", playerNumber);
            }
            else
            {
                MovePlayerSelectionByDirection("up", playerNumber);
            }
        }
    }

    private void MovePlayerSelectionByDirection(string direction, int playerNumber)
    {
        if (!playerSelectsCanMove[playerNumber - 1])
        {
            return;
        }

        Selectable toSelect = null;

        if (direction == "left") toSelect = playersSelected[playerNumber - 1].FindSelectableOnLeft();
        if (direction == "right") toSelect = playersSelected[playerNumber - 1].FindSelectableOnRight();
        if (direction == "up") toSelect = playersSelected[playerNumber - 1].FindSelectableOnUp();
        if (direction == "down") toSelect = playersSelected[playerNumber - 1].FindSelectableOnDown();

        if (toSelect != null)
        {
            playersSelected[playerNumber - 1] = (Button)toSelect;
            ShowSelectForPlayer((Button)toSelect, playerNumber);

            StartCoroutine(WaitUntilPlayerSelectCanMove(playerNumber));
        }
    }

    private void ShowSelectForPlayer(Button toSelect, int playerNumber)
    {
        playerSelectedImages[playerNumber - 1].transform.position = toSelect.transform.position;
    }

    public void SubmitForPlayerSelected(int playerNumber)
    {
        Debug.Log(playersSelected.Count + " number " + (playerNumber - 1));
        playersSelected[playerNumber - 1].onClick.Invoke();
    }

    public IEnumerator WaitUntilPlayerSelectCanMove(int playerNumber)
    {
        playerSelectsCanMove[playerNumber - 1] = false;
        yield return new WaitForSecondsRealtime(selectMoveWaitTime);
        playerSelectsCanMove[playerNumber - 1] = true;
    }

    private void SetAllIconColors()
    {
        for (int i = 0; i < 4; i++)
        {
            playerSelectedImages[i].color = GameConstants.UIColors.ParseFromName(selectedColors[i].text, 150);
        }
    }
}
