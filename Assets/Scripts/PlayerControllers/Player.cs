using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Sword {

    protected override void Move()
    {
        Vector3 moveVector = new Vector3(0, 0, 0);

        if (owner.usesKeyboardControls)
        {
            moveVector = MoveForKeyboard();
        }
        else
        {
            moveVector = MoveForController();
        }

        if (normalizedBoostVelocity)
        {
            moveVector = moveVector.normalized * moveSpeed;
        }

        if (moveVector.y == 0)
        {
            moveVector += new Vector3(0, rigbod.velocity.y, 0);
        }

        // This makes it so you don't stop on a dime when letting go of the stick, but also ruins moving on the ground and attack while standing still.
        //if (moveVector.x == 0 && moveVector.z == 0)
        //{
        //     moveVector += new Vector3(rigbod.velocity.x, 0, 0);
        //    moveVector += new Vector3(0, 0, rigbod.velocity.z);
        //}
        
        rigbod.velocity = moveVector;
    }

    private Vector3 MoveForKeyboard()
    {
        Vector3 moveVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W))
        {
            moveVector += new Vector3(0, 0, moveSpeed);
        }

        if (Input.GetKey(KeyCode.S))
        {
            moveVector += new Vector3(0, 0, -moveSpeed);
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveVector += new Vector3(-moveSpeed, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            moveVector += new Vector3(moveSpeed, 0, 0);
        }

        moveVector = moveVector.normalized * moveSpeed;
        Debug.Log(System.DateTime.Now + moveVector.ToString());

        if (boost > 0 && Input.GetKey(KeyCode.Space))
        {
            moveVector += new Vector3(0, moveSpeed, 0);
            boosting = true;
        }

        if (boost > 0 && Input.GetKey(KeyCode.LeftControl))
        {
            moveVector += new Vector3(0, -moveSpeed, 0);
            boosting = true;
        }

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            boosting = false;
            if (boostDelayCoroutine != null)
            {
                StopCoroutine(boostDelayCoroutine);
            }

            boostDelayCoroutine = StartCoroutine(WaitPostBoostMS(boostRegenDelayMS));
        }

        return moveVector;
    }

    private Vector3 MoveForController()
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
            if (boostDelayCoroutine != null)
            {
                StopCoroutine(boostDelayCoroutine);
            }

            boostDelayCoroutine = StartCoroutine(WaitPostBoostMS(boostRegenDelayMS));
        }

        return moveVector;
    }

    protected override void AttackInput()
    {
        if (!attacking && Input.GetButtonDown(playerNumber + "Swing") || (owner.usesKeyboardControls && Input.GetMouseButtonDown(0)))
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
        if (Input.GetButton(playerNumber + "Block") || (owner.usesKeyboardControls && Input.GetMouseButtonDown(1)))
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
