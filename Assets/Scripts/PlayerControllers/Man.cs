using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Man : MonoBehaviour {

    public UIManager ui;

    public int playerNumber;

    public bool isCameraTarget = true;

    public float maxHealth;
    public float health;
    public float invinceTimeMS;

    public string color;
    public string weapon;
    public string hat;
    public string misc;
    public string skin;

    protected System.DateTime hitTime;

	// Use this for initialization
	public virtual void Start () {
        health = maxHealth;
        hitTime = System.DateTime.Now;
        ui = FindObjectOfType<UIManager>();

        SetSkin(skin);
        SetColor(color);
        SetWeapon(weapon);
        SetMisc(misc);
        SetHat(hat);
    }

    void SetColor(string c)
    {
        Color toSet = Color.gray;
        if (c == "Red")
        {
            toSet = GameConstants.PlayerColors.red;
        }
        if (c == "Blue")
        {
            toSet = GameConstants.PlayerColors.blue;
        }
        if (c == "Green")
        {
            toSet = GameConstants.PlayerColors.green;
        }
        if (c == "Yellow")
        {
            toSet = GameConstants.PlayerColors.yellow;
        }
        if (c == "Orange")
        {
            toSet = GameConstants.PlayerColors.orange;
        }
        if (c == "Purple")
        {
            toSet = GameConstants.PlayerColors.purple;
        }

        transform.Find("Body/Body Spine").GetComponent<MeshRenderer>().material.color = toSet;
        transform.Find("Body/Body Pelvis").GetComponent<MeshRenderer>().material.color = toSet;
        transform.Find("Body/Body Pelvis/Fill").GetComponent<MeshRenderer>().material.color = toSet;

        for (int i = 0; i < transform.Find("Body/Body Head/").childCount; i++)
        {
            transform.Find("Body/Body Head/").GetChild(i).GetComponent<MeshRenderer>().material.color = toSet;
        }

        for (int i = 0; i < transform.Find("Body/Body Spine/Items/").childCount; i++)
        {
            transform.Find("Body/Body Spine/Items/").GetChild(i).GetComponent<MeshRenderer>().material.color = toSet;
        }
    }

    void SetWeapon(string w)
    {
        transform.Find("Weapon/" + w).gameObject.SetActive(true);
    }

    void SetHat(string h)
    {
        if (h != "None")
        {
            transform.Find("Body/Body Head/" + h).gameObject.SetActive(true);
        }
    }

    void SetMisc(string m)
    {
        if (m != "None")
        {
            transform.Find("Body/Body Spine/Items/" + m).gameObject.SetActive(true);
        }
    }

    void SetSkin(string s)
    {
        Color toSet = Color.gray;
        if (s == "Light")
        {
            toSet = GameConstants.SkinColors.light;
        }
        if (s == "Medium")
        {
            toSet = GameConstants.SkinColors.medium;
        }
        if (s == "Dark")
        {
            toSet = GameConstants.SkinColors.dark;
        }
        if (s == "Green")
        {
            toSet = GameConstants.SkinColors.green;
        }

        Transform thisBody = transform.Find("Body");
        for (int i = 0; i < thisBody.childCount; i++)
        {
            Transform childBody = thisBody.GetChild(i);
            childBody.GetComponent<MeshRenderer>().material.color = toSet;
            for (int j = 0; j < childBody.childCount; j++)
            {
                childBody.GetChild(j).GetComponent<MeshRenderer>().material.color = toSet;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TakeDamage(float damage)
    {
        if (damage > 0 && health > 0 && (System.DateTime.Now - hitTime).TotalMilliseconds > invinceTimeMS)
        {
            ChangeHealth(health - damage);
            ui.ShowHurtImage(playerNumber, damage);
            hitTime = System.DateTime.Now;
            //Debug.Log("Player " + playerNumber + ": " + damage + " damage taken");
        }
    }

    public void ChangeHealth(float newHealth)
    {
        health = newHealth;
        if (health <= 0)
        {
            health = 0;
            ui.PlayerDead(playerNumber);
            CameraFollow camera = FindObjectOfType<CameraFollow>();
            if (camera != null)
            {
                camera.Delete(this);
            }
            ExtraDeathEffects();
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        ui.ChangeHealth(health / maxHealth, playerNumber);
    }

    protected virtual void ExtraDeathEffects()
    {
        // None
    }
}
