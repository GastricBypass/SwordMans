using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour {

    public UIManager ui;
    public Man owner;

    public int playerNumber;

    public float moveSpeed = 12;
    public float maxBoost = 10;
    public float boost = 10;
    public bool normalizedBoostVelocity = false; // makes boost normalize with total velocity. Boost will be additive when this is false
    public float boostDrainSpeed = 0.15f;
    public float boostRegenSpeed = 0.10f;
    public float boostRegenDelayMS = 1000;

    public float attackSwingTimeMS = 250;
    public Vector3 attackSwingSpeed = new Vector3(0, 0, -1000);
    public float attackCost = 0;
    public float attackDowntimeMS = 250;

    public float blockStrength = 1500;

    protected Rigidbody rigbod;
    protected System.DateTime startTime;

    public bool attacking = false;
    public bool canAttack = true;
    public bool shouldAttack = false;

    protected bool blocking = false;

    protected bool boosting = false;
    protected bool shouldRegenBoost = true;

    protected virtual void Start() {

        rigbod = this.GetComponent<Rigidbody>();
        ui = FindObjectOfType<UIManager>();
        playerNumber = owner.playerNumber;
    }

    protected virtual void Update()
    {
        if (!ui.paused)
        {
            boosting = false;

            if (canAttack && (attackCost == 0 || boost > 0))
            {
                AttackInput();
            }

            if (!attacking)
            {
                BlockInput();
            }

            if (owner.health <= 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    void FixedUpdate() {
        if (!ui.paused)
        {
            Move();

            Attack();

            Block();

            Boost();
        }
    }

    protected virtual void Move()
    {
        // Implement to add functionality
    }

    protected virtual void AttackInput()
    {
        
    }

    protected virtual void Attack()
    {
        // Implement to add functionality
    }

    protected virtual void BlockInput()
    {

    }

    protected virtual void Block()
    {
        // Implement to add functionality
    }

    // Drains boost at the specified rate per frame when boosting
    protected void Boost()
    {
        if (boosting)
        {
            ChangeBoost(boost - boostDrainSpeed);
            
            if (boost <= 0)
            {
                boosting = false;
                boost = 0;
                StartCoroutine(WaitPostBoostMS(boostRegenDelayMS));
            }
        }

        else if (shouldRegenBoost)
        {
            RegenBoost();
        }
    }

    // Regenerated the boost at the specified rate per frame
    protected void RegenBoost()
    {
        if (boost < maxBoost)
        {
            ChangeBoost(boost + boostRegenSpeed);
            
            if (boost > maxBoost)
            {
                boost = maxBoost;
            }
        }
    }

    // Updates the boost to the input amount and changes the UI boost meter
    protected void ChangeBoost(float newBoost)
    {
        boost = newBoost;
        ui.ChangeBoost(boost / maxBoost, owner.playerNumber);
    }

    protected IEnumerator WaitPostAttackMS(float ms)
    {
        canAttack = false;
        yield return new WaitForSeconds(ms / 1000f);
        canAttack = true;
    }

    protected IEnumerator WaitAttackMS(float ms)
    {
        attacking = true;
        yield return new WaitForSeconds(ms / 1000f);
        attacking = false;
    }

    protected IEnumerator WaitPostBoostMS(float ms)
    {
        shouldRegenBoost = false;
        yield return new WaitForSeconds(ms / 1000f);
        shouldRegenBoost = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.GetComponent<Sword>() != null)
        {
            attacking = false;
            rigbod.velocity = -rigbod.velocity;
        }
    }
}
