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

    private System.DateTime hitTime;

	// Use this for initialization
	void Start () {
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

        for (int i = 0; i < transform.Find("Body/Body Head/").childCount; i++)
        {
            transform.Find("Body/Body Head/").GetChild(i).GetComponent<MeshRenderer>().material.color = toSet;
        }

        for (int i = 0; i < transform.Find("Body/Body Spine/").childCount; i++)
        {
            transform.Find("Body/Body Spine/").GetChild(i).GetComponent<MeshRenderer>().material.color = toSet;
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
            transform.Find("Body/Body Spine/" + m).gameObject.SetActive(true);
        }
    }

    void SetSkin(string s)
    {
        Color toSet = Color.gray;
        if (s == "Light")
        {
            toSet = new Color(255f / 255f, 200f / 255f, 180f / 255f);
        }
        if (s == "Medium")
        {
            toSet = new Color(200f / 255f, 140f / 255f, 70f / 255f);
        }
        if (s == "Dark")
        {
            toSet = new Color(60f / 255f, 30f / 255f, 0f / 255f);
        }
        if (s == "Green")
        {
            toSet = new Color(50f / 255f, 100f / 255f, 35f / 255f);
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
            Debug.Log("Player " + this.playerNumber + ": " + damage + " damage taken");
            ChangeHealth(health - damage);
            ui.ShowHurtImage(playerNumber, damage);
            hitTime = System.DateTime.Now;
        }
    }

    public void ChangeHealth(float newHealth)
    {
        health = newHealth;
        if (health <= 0)
        {
            health = 0;
            CameraFollow camera = FindObjectOfType<CameraFollow>();
            camera.Delete(this);
        }
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        ui.ChangeHealth(health / maxHealth, playerNumber);
    }
}
