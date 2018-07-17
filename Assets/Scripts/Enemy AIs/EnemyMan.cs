using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMan : Man {

    public bool isEnemy = true;
    public bool disappearsOnDeath = true;
    public bool isBoss;
    public bool hasSamePlayerCustomizationOptions = false;
    public bool isVersusEnemy = false;

	// Use this for initialization
	public override void Start ()
    {
        if (hasSamePlayerCustomizationOptions)
        {
            base.Start();
        }
        else
        {
            health = maxHealth;
            ui = FindObjectOfType<UIManager>();
        }

        if (isBoss)
        {
            StartCoroutine(WaitToSetHealth(0.1f));
        }
    }

    public IEnumerator WaitToSetHealth(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        maxHealth = maxHealth + (maxHealth * ui.gsm.numberOfPlayers / 2f);
        health = maxHealth;
        ui.ChangeHealth(1, this.playerNumber);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    public override bool CanUpdateUiHealth()
    {
        if (isBoss && isEnemy)
        {
            return base.CanUpdateUiHealth();
        }
        else return isVersusEnemy;
    }

    protected override void ExtraDeathEffects()
    {
        base.ExtraDeathEffects();
        if (disappearsOnDeath)
        {
            Destroy(this.gameObject);
        }

        if (isBoss)
        {

        }
    }
}
