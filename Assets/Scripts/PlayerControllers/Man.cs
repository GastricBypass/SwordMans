﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Man : MonoBehaviour {

    public UIManager ui;

    public int playerNumber;

    public bool isCameraTarget = true;

    public float maxHealth;
    public float health;
    public float invinceTimeMS;

    public bool invincible = false; // not used with the i-frames that come from invinceTimeMS

    public string color;
    public string weapon;
    public string hat;
    public string misc;
    public string skin;

    protected bool dead = false;

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

        // TODO: temporarily disabled for non-online play

        //if (playerNumber == 0) // This will break all the npcs
        //{
        //    // also probs don't need this
        //    Debug.Log("Player " + (Network.connections.Length + 1) + " entered the game.");
        //    playerNumber = Network.connections.Length + 1; // to set the player number as players enter the game.
        //}

        //this.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToServer);
        //CmdSetAuthority(); 
    }

    //[Command]
    //void CmdSetAuthority()
    //{
    //    GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToServer);
    //}

    void SetColor(string c)
    {
        Color toSet = GameConstants.PlayerColors.ParseFromName(c);

        transform.Find("Body/Body Spine").GetComponent<MeshRenderer>().material.color = toSet;
        transform.Find("Body/Body Pelvis").GetComponent<MeshRenderer>().material.color = toSet;
        transform.Find("Body/Body Pelvis/Fill").GetComponent<MeshRenderer>().material.color = toSet;

        Transform head = transform.Find("Body/Body Head/");
        for (int i = 0; i < head.childCount; i++)
        {
            if (head.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null)
            {
                head.GetChild(i).GetComponent<SkinnedMeshRenderer>().material.color = toSet;
            }
            else if (head.GetChild(i).GetComponent<MeshRenderer>() != null)
            {
                head.GetChild(i).GetComponent<MeshRenderer>().material.color = toSet;
            }
        }

        Transform spine = transform.Find("Body/Body Spine/Items/");
        for (int i = 0; i < spine.childCount; i++)
        {
            if (spine.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null)
            {
                spine.GetChild(i).GetComponent<SkinnedMeshRenderer>().material.color = toSet;
            }
            else if (spine.GetChild(i).GetComponent<MeshRenderer>().material.color != null)
            {
                spine.GetChild(i).GetComponent<MeshRenderer>().material.color = toSet;
            }
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
        Color toSet = GameConstants.SkinColors.ParseFromName(s);

        Transform thisBody = transform.Find("Body");
        for (int i = 0; i < thisBody.childCount; i++)
        {
            Transform childBody = thisBody.GetChild(i);
            childBody.GetComponent<MeshRenderer>().material.color = toSet;
            for (int j = 0; j < childBody.childCount; j++)
            {
                if (childBody.GetChild(j).GetComponent<MeshRenderer>() != null)
                {
                    childBody.GetChild(j).GetComponent<MeshRenderer>().material.color = toSet;
                }
            }
        }
    }

    public void TakeDamage(float damage, bool alwaysDealsDamage = false)
    {
        if (!invincible && damage > 0 && health > 0 && (alwaysDealsDamage || (System.DateTime.Now - hitTime).TotalMilliseconds > invinceTimeMS))
        {
            ChangeHealth(health - damage);
            ui.ShowHurtImage(playerNumber, damage);

            hitTime = System.DateTime.Now;
            //Debug.Log("Player " + playerNumber + ": " + damage + " damage taken");
        }
    }

    public void ChangeHealth(float newHealth)
    {
        if (dead)
        {
            return;
        }
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
            dead = true;
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
