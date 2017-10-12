using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AISwordMan : AIEnemy {

    public float attackRange = 1;
    public float aggression = 0.5f; // 0 to 1: 1 = attack at every opportunity, 0 = never attack
    public float cowardice = 0.5f; // how likely they are to run away at lower health (changes aggression)
    public float zeal = 0.5f; // how likely they are to continue attacking when their target is at low health (changed aggression)
    public Vector2 aggressionSwitchRange = new Vector2(2, 5);

    private bool aggressive;
    public float aggressionFactor;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
        StartCoroutine(AggressionSwitch(aggressionSwitchRange.y));
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        base.Move();
        if (!aggressive)
        {
            rigbod.velocity = new Vector3(-rigbod.velocity.x, rigbod.velocity.y, -rigbod.velocity.z);
        }
    }

    protected override void AttackInput()
    {
        if (!attacking && target != null && (target.transform.position - this.transform.position).magnitude < attackRange && Random.Range(0f, 1f) < aggressionFactor)
        {
            shouldAttack = true;
            StartCoroutine(WaitAttackMS(attackSwingTimeMS));

            if (attackCost > 0)
            {
                ChangeBoost(boost - attackCost);
                StartCoroutine(WaitPostBoostMS(boostRegenDelayMS));
            }
        }
    }

    protected override void Attack()
    {
        if (attacking)
        {
            rigbod.AddRelativeForce(attackSwingSpeed);
        }

        if (shouldAttack && !attacking)
        {
            shouldAttack = false;
            StartCoroutine(WaitPostAttackMS(attackDowntimeMS));
        }
    }

    protected override void BlockInput()
    {
        if (target != null && (target.transform.position - this.transform.position).magnitude < attackRange && Random.Range(0f, 1f) > aggressionFactor)
        {
            blocking = true;
        }
        else
        {
            blocking = false;
        }
    }

    protected override void Block()
    {
        if (blocking)
        {
            Vector3 tug = Vector3.up * -blockStrength;

            rigbod.AddForceAtPosition(transform.TransformPoint(Vector3.up), tug);
            rigbod.AddForceAtPosition(transform.TransformPoint(-Vector3.up), -tug);
        }
    }

    protected IEnumerator AggressionSwitch(float switchTime)
    {
        ChangeAggression();
        aggressive = Random.Range(0f, 1f) < aggressionFactor;
        yield return new WaitForSeconds(switchTime);
        StartCoroutine(AggressionSwitch(Random.Range(aggressionSwitchRange.x, aggressionSwitchRange.y)));
    }

    protected void ChangeAggression()
    {
        float cowardiceFactor = cowardice - (owner.health / owner.maxHealth * cowardice);
        float zealFactor = 0;
        if (target != null)
        {
            BodyPart targetBodyPart = target.GetComponent<BodyPart>();
            if (targetBodyPart != null)
            {
                zealFactor = zeal - (targetBodyPart.owner.health / targetBodyPart.owner.maxHealth * zeal);
            }
        }

        aggressionFactor = aggression + zealFactor - cowardiceFactor;
    }
}
