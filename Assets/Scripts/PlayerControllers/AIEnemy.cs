using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : Sword {

    public GameObject target;
    public float aggroDistance;

    protected override void Start()
    {
        base.Start();
        target = FindTarget(); 
    }

    protected override void Move()
    {
        if ((target.transform.position - this.transform.position).magnitude < aggroDistance)
        {
            Vector3 moveVector = new Vector3(0, 0, 0);

            if (target.transform.position.x > rigbod.transform.position.x)
            {
                moveVector += new Vector3(moveSpeed, 0, 0);
            }

            if (target.transform.position.z > rigbod.transform.position.z)
            {
                moveVector += new Vector3(0, 0, moveSpeed);
            }

            if (target.transform.position.x < rigbod.transform.position.x)
            {
                moveVector += new Vector3(-moveSpeed, 0, 0);
            }

            if (target.transform.position.z < rigbod.transform.position.z)
            {
                moveVector += new Vector3(0, 0, -moveSpeed);
            }

            if (boost > 0 && target.transform.position.y > rigbod.transform.position.y + 4)
            {
                moveVector += new Vector3(0, moveSpeed, 0);
                boosting = true;
            }

            if (boost > 0 && target.transform.position.y < rigbod.transform.position.y - 4)
            {
                moveVector += new Vector3(0, -moveSpeed, 0);
                boosting = true;
            }

            if (!(boost > 0) && !(target.transform.position.y > rigbod.transform.position.y + 4) && !(target.transform.position.y < rigbod.transform.position.y - 4))
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
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Block()
    {
        base.Block();
    }

    GameObject FindTarget()
    {
        GameObject toReturn = null;

        BodyPart[] possibleTargets = FindObjectsOfType<BodyPart>();

        for (int i = 0; i < possibleTargets.Length; i++)
        {
            if (possibleTargets[i].owner.playerNumber != this.owner.playerNumber)
            {
                toReturn = possibleTargets[i].gameObject;
                break;
            }
        }

        return toReturn;
    }
}