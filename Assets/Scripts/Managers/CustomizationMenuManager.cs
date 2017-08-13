using System.Collections;
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

    public Text color1;
    public Text weapon1;
    public Text hat1;
    public Text misc1;
    public Text skin1;

    public Text color2;
    public Text weapon2;
    public Text hat2;
    public Text misc2;
    public Text skin2;

    public Text color3;
    public Text weapon3;
    public Text hat3;
    public Text misc3;
    public Text skin3;

    public Text color4;
    public Text weapon4;
    public Text hat4;
    public Text misc4;
    public Text skin4;

    // Use this for initialization
    void Start ()
    {
        // Player 1 presets
        color1.text = manager.gsm.player1Color;
        weapon1.text = manager.gsm.player1Weapon;
        hat1.text = manager.gsm.player1Hat;
        misc1.text = manager.gsm.player1Misc;
        skin1.text = manager.gsm.player1Skin;

        colorIndeces[0] = colors.IndexOf(color1.text);
        weaponIndeces[0] = weapons.IndexOf(weapon1.text);
        hatIndeces[0] = hats.IndexOf(hat1.text);
        miscIndeces[0] = misc.IndexOf(misc1.text);
        skinIndeces[0] = skins.IndexOf(skin1.text);

        // Player 2 presets
        color2.text = manager.gsm.player2Color;
        weapon2.text = manager.gsm.player2Weapon;
        hat2.text = manager.gsm.player2Hat;
        misc2.text = manager.gsm.player2Misc;
        skin2.text = manager.gsm.player2Skin;

        colorIndeces[1] = colors.IndexOf(color2.text);
        weaponIndeces[1] = weapons.IndexOf(weapon2.text);
        hatIndeces[1] = hats.IndexOf(hat2.text);
        miscIndeces[1] = misc.IndexOf(misc2.text);
        skinIndeces[1] = skins.IndexOf(skin2.text);

        // Player 3 presets
        color3.text = manager.gsm.player3Color;
        weapon3.text = manager.gsm.player3Weapon;
        hat3.text = manager.gsm.player3Hat;
        misc3.text = manager.gsm.player3Misc;
        skin3.text = manager.gsm.player3Skin;


        colorIndeces[2] = colors.IndexOf(color3.text);
        weaponIndeces[2] = weapons.IndexOf(weapon3.text);
        hatIndeces[2] = hats.IndexOf(hat3.text);
        miscIndeces[2] = misc.IndexOf(misc3.text);
        skinIndeces[2] = skins.IndexOf(skin3.text);

        // Player 4 presets
        color4.text = manager.gsm.player4Color;
        weapon4.text = manager.gsm.player4Weapon;
        hat4.text = manager.gsm.player4Hat;
        misc4.text = manager.gsm.player4Misc;
        skin4.text = manager.gsm.player4Skin;

        colorIndeces[3] = colors.IndexOf(color4.text);
        weaponIndeces[3] = weapons.IndexOf(weapon4.text);
        hatIndeces[3] = hats.IndexOf(hat4.text);
        miscIndeces[3] = misc.IndexOf(misc4.text);
        skinIndeces[3] = skins.IndexOf(skin4.text);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
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
        if (playerNum == 1)
        {
            color1.text = colors[index];
            manager.gsm.player1Color = color1.text;
        }
        if (playerNum == 2)
        {
            color2.text = colors[index];
            manager.gsm.player2Color = color2.text;
        }
        if (playerNum == 3)
        {
            color3.text = colors[index];
            manager.gsm.player3Color = color3.text;
        }
        if (playerNum == 4)
        {
            color4.text = colors[index];
            manager.gsm.player4Color = color4.text;
        }
    }

    private void SetWeapon(int index, int playerNum)
    {
        if (playerNum == 1)
        {
            weapon1.text = weapons[index];
            manager.gsm.player1Weapon = weapon1.text;
        }
        if (playerNum == 2)
        {
            weapon2.text = weapons[index];
            manager.gsm.player2Weapon = weapon2.text;
        }
        if (playerNum == 3)
        {
            weapon3.text = weapons[index];
            manager.gsm.player3Weapon = weapon3.text;
        }
        if (playerNum == 4)
        {
            weapon4.text = weapons[index];
            manager.gsm.player4Weapon = weapon4.text;
        }
    }

    private void SetHat(int index, int playerNum)
    {
        if (playerNum == 1)
        {
            hat1.text = hats[index];
            manager.gsm.player1Hat = hat1.text;
        }
        if (playerNum == 2)
        {
            hat2.text = hats[index];
            manager.gsm.player2Hat = hat2.text;
        }
        if (playerNum == 3)
        {
            hat3.text = hats[index];
            manager.gsm.player3Hat = hat3.text;
        }
        if (playerNum == 4)
        {
            hat4.text = hats[index];
            manager.gsm.player4Hat = hat4.text;
        }
    }

    private void SetMisc(int index, int playerNum)
    {
        if (playerNum == 1)
        {
            misc1.text = misc[index];
            manager.gsm.player1Misc = misc1.text;
        }
        if (playerNum == 2)
        {
            misc2.text = misc[index];
            manager.gsm.player2Misc = misc2.text;
        }
        if (playerNum == 3)
        {
            misc3.text = misc[index];
            manager.gsm.player3Misc = misc3.text;
        }
        if (playerNum == 4)
        {
            misc4.text = misc[index];
            manager.gsm.player4Misc = misc4.text;
        }
    }

    private void SetSkin(int index, int playerNum)
    {
        if (playerNum == 1)
        {
            skin1.text = skins[index];
            manager.gsm.player1Skin = skin1.text;
        }
        if (playerNum == 2)
        {
            skin2.text = skins[index];
            manager.gsm.player2Skin = skin2.text;
        }
        if (playerNum == 3)
        {
            skin3.text = skins[index];
            manager.gsm.player3Skin = skin3.text;
        }
        if (playerNum == 4)
        {
            skin4.text = skins[index];
            manager.gsm.player4Skin = skin4.text;
        }
    }
}
