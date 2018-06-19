using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Man : MonoBehaviour {

    public UIManager ui;

    public int playerNumber;

    public bool isCameraTarget = true;

    public bool usesKeyboardControls = false;
    public float maxHealth;
    public float health;
    public float invinceTimeMS;

    public bool invincible = false; // not used with the i-frames that come from invinceTimeMS

    public string color;
    public string weapon;
    public string hat;
    public string misc;
    public string skin;

    public bool hasExploded;
    public bool isInDamagingArea; // only used for AIs to determine if they're in a damaging area.

    protected bool dead = false;

    protected System.DateTime hitTime;
    
    public List<GameObject> objectsThatRecentlyDealtDamage;

    // Achievement: I Shouldn't Be Alive
    private float hpRecoveredThisLife;
    // Achievement: Air Time
    public float boostUsedSinceLanding;

	// Use this for initialization
	public virtual void Start () {
        //health = maxHealth;
        ui = FindObjectOfType<UIManager>();

        SetSkin(skin);
        SetColor(color);
        SetWeapon(weapon);
        SetMisc(misc);
        SetHat(hat);

        if (CanUpdateUiHealth())
        {
            ui.ChangeHealth(health / maxHealth, playerNumber);
        }

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

    public void ResetAllCosmetics()
    {
        ResetHat();
        ResetMisc();
        ResetWeapon();
    }

    public void ResetHat()
    {
        Transform head = transform.Find(GameConstants.Unlocks.hatsLocation);
        for (int i = 0; i < head.childCount; i++)
        {
            Transform child = head.GetChild(i);
            if (child.name != "Skull (not a hat)")
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ResetMisc()
    {
        Transform spine = transform.Find(GameConstants.Unlocks.miscLocation);
        for (int i = 0; i < spine.childCount; i++)
        {
            spine.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void ResetWeapon()
    {
        Transform weapon = transform.Find("Weapon");
        for (int i = 0; i < weapon.childCount; i++)
        {
            weapon.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void SetColor(string c)
    {
        Color toSet = GameConstants.PlayerColors.ParseFromName(c);

        transform.Find("Body/Body Spine").GetComponent<MeshRenderer>().material.color = toSet;
        transform.Find("Body/Body Pelvis").GetComponent<MeshRenderer>().material.color = toSet;
        transform.Find("Body/Body Pelvis/Fill").GetComponent<MeshRenderer>().material.color = toSet;

        Transform head = transform.Find(GameConstants.Unlocks.hatsLocation);
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

        Transform spine = transform.Find(GameConstants.Unlocks.miscLocation);
        for (int i = 0; i < spine.childCount; i++)
        {
            if (spine.GetChild(i).GetComponent<SkinnedMeshRenderer>() != null)
            {
                spine.GetChild(i).GetComponent<SkinnedMeshRenderer>().material.color = toSet;
            }
            else if (spine.GetChild(i).GetComponent<MeshRenderer>() != null)
            {
                spine.GetChild(i).GetComponent<MeshRenderer>().material.color = toSet;
            }
        }
    }

    public void SetWeapon(string w)
    {
        transform.Find("Weapon/" + w).gameObject.SetActive(true);
    }

    public void SetHat(string h)
    {
        if (h != "None")
        {
            transform.Find("Body/Body Head/" + h).gameObject.SetActive(true);
        }
    }

    public void SetMisc(string m)
    {
        if (m != "None")
        {
            transform.Find("Body/Body Spine/Items/" + m).gameObject.SetActive(true);
        }
    }

    public void SetSkin(string s)
    {
        Color toSet = GameConstants.SkinColors.ParseFromName(s);

        Transform thisBody = transform.Find("Body");
        for (int i = 0; i < thisBody.childCount; i++)
        {
            Transform childBody = thisBody.GetChild(i);

            if (childBody.gameObject.name == "Body Spine" || childBody.gameObject.name == "Body Pelvis")
            {
                // We don't need to set the color of the spine or pelvis
                continue;
            }

            MeshRenderer childMesh = childBody.GetComponent<MeshRenderer>();
            if (childMesh != null)
            {
                childMesh.material.color = toSet;
                if (toSet.a == 0)
                {
                    childMesh.enabled = false;
                }
                else
                {
                    childMesh.enabled = true;
                }
            }

            if (childBody.gameObject.name == "Body Head")
            {
                // We don't need to set the color of the children of the head
                continue;
            }

            for (int j = 0; j < childBody.childCount; j++)
            {
                MeshRenderer mesh = childBody.GetChild(j).GetComponent<MeshRenderer>();
                if (mesh != null)
                {
                    mesh.material.color = toSet;
                    if (toSet.a == 0)
                    {
                        mesh.enabled = false;
                    }
                    else
                    {
                        mesh.enabled = true;
                    }
                }
            }
        }
    }

    public bool CanTakeDamage(bool alwaysDealsDamage = false)
    {
        return (!invincible && health > 0);
    }

    public void TakeDamage(float damage, bool alwaysDealsDamage = false)
    {
        if (damage > 0 && CanTakeDamage(alwaysDealsDamage))
        {
            ChangeHealth(health - damage);

            // Stat: damage_dealt
            ui.gsm.steam.AddDamageDealt(damage);
            
            //Debug.Log("Player " + playerNumber + ": " + damage + " damage taken");
        }
    }

    public void ChangeHealth(float newHealth)
    {
        if (dead)
        {
            return;
        }

        AddHpRecovered(newHealth);
        
        float oldHealth = health;
        health = newHealth;
        if (health <= 0)
        {
            health = 0;
            if (!hasExploded)
            {
                ui.PlayerDead(playerNumber);
            }
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

        if (CanUpdateUiHealth())
        {
            ui.ChangeHealth(health / maxHealth, playerNumber);
            if (health < oldHealth)
            {
                ui.ShowHurtImage(playerNumber, oldHealth - health);
            }
        }
    }

    public virtual bool CanUpdateUiHealth()
    {
        return true;
    }

    public void AddObjectThatRecentlyDealtDamage(GameObject obj)
    {
        objectsThatRecentlyDealtDamage.Add(obj);

        StartCoroutine(RemoveObjectThatRecentlyDealtDamageAfterTime(obj, invinceTimeMS));
    }

    private IEnumerator RemoveObjectThatRecentlyDealtDamageAfterTime(GameObject obj, float ms)
    {
        yield return new WaitForSeconds(ms / 1000f);

        objectsThatRecentlyDealtDamage.Remove(obj);
    }

    // Achievement: I Shouldn't Be Alive
    private void AddHpRecovered(float newHealth)
    {
        if (newHealth > health)
        {
            float toAdd = 0;
            if (newHealth > maxHealth)
            {
                toAdd = maxHealth - health;
            }
            else
            {
                toAdd = newHealth - health;
            }

            hpRecoveredThisLife += toAdd;

            if (hpRecoveredThisLife >= 1000)
            {
                // Achievement: I Shouldn't Be Alive
                ui.gsm.steam.UnlockAchievement(GameConstants.AchievementId.I_SHOULDNT_BE_ALIVE);
            }
        }
    }

    // Achievement: Air Time
    public void AddBoostUsed(float boostUsed)
    {
        boostUsedSinceLanding += boostUsed;

        if (boostUsedSinceLanding >= 50)
        {
            // Achievement: Air Time
            ui.gsm.steam.UnlockAchievement(GameConstants.AchievementId.AIR_TIME);
        }
    }

    protected virtual void ExtraDeathEffects()
    {
        // None
    }
}
