using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMan : Man {

    public bool isEnemy = true;
    public bool disappearsOnDeath = true;
    public bool isBoss;
    public bool hasSamePlayerCustomizationOptions = false;

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
            hitTime = System.DateTime.Now;
            ui = FindObjectOfType<UIManager>();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    protected override void ExtraDeathEffects()
    {
        base.ExtraDeathEffects();
        if (disappearsOnDeath)
        {
            Destroy(this.gameObject);
        }
    }
}
