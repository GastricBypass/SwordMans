using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Sword {

    protected override void Move()
    {
        Vector3 moveVector = new Vector3(0, 0, 0);

        float x = Input.GetAxis(playerNumber + "Horizontal");
        float z = Input.GetAxis(playerNumber + "Vertical");
        moveVector += new Vector3(x, 0, -z).normalized * moveSpeed;

        if (boost > 0 && Input.GetButton(playerNumber + "Rise"))
        {
            moveVector += new Vector3(0, moveSpeed, 0);
            boosting = true;
        }

        if (boost > 0 && Input.GetButton(playerNumber + "Lower"))
        {
            moveVector += new Vector3(0, -moveSpeed, 0);
            boosting = true;
        }

        if (Input.GetButtonUp(playerNumber + "Rise") || Input.GetButtonUp(playerNumber + "Lower"))
        {
            boosting = false;
            if (shouldRegenBoost)
            {
                StartCoroutine(WaitPostBoostMS(boostRegenDelayMS));
            }
        }

        if (normalizedBoostVelocity)
        {
            moveVector = moveVector.normalized * moveSpeed;
        }

        rigbod.velocity = moveVector;
    }

    protected override void AttackInput()
    {
        if (!attacking && Input.GetButtonDown(playerNumber + "Swing"))
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
        if (Input.GetButton(playerNumber + "Block"))
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
}
