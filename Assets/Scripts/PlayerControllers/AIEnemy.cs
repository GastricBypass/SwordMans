﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIEnemy : Sword {

    public GameObject target;
    public bool changeTargetsAfterDeath = false;
    public float aggroDistance = 15;
    public float aggroWaitTime = 0.1f;

    private bool waiting = false;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Move()
    {
        if (!waiting && target == null)
        {
            StartCoroutine(WaitToFindTarget(aggroWaitTime));
        }

        if (changeTargetsAfterDeath && target != null)
        {
            BodyPart targetBodyPart = target.GetComponent<BodyPart>();
            if (targetBodyPart != null)
            {
                if (targetBodyPart.owner.health <= 0)
                {
                    target = null;
                }
            }
        }

        Vector3 moveVector = new Vector3(0, 0, 0);

        if (target == null)
        {
            rigbod.velocity = moveVector;
            return;
        }

        else if (target != null && (target.transform.position - this.transform.position).magnitude < aggroDistance)
        {
            moveVector = new Vector3(target.transform.position.x, 0, target.transform.position.z) - new Vector3(this.transform.position.x, 0, this.transform.position.z);
            moveVector = moveVector.normalized * moveSpeed;

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

            if (boosting && !(target.transform.position.y > rigbod.transform.position.y + 4) && !(target.transform.position.y < rigbod.transform.position.y - 4))
            {
                shouldRegenBoost = false; // Probably redundant, just trying things
                EndBoost();
            }

            if (normalizedBoostVelocity)
            {
                moveVector = moveVector.normalized * moveSpeed;
            }

            if (moveVector.y == 0)
            {
                moveVector += new Vector3(0, rigbod.velocity.y, 0);
            }
        }

        else
        {
            target = null;
        }

        rigbod.velocity = moveVector;
    }

    protected override void Attack()
    {
        base.Attack();
    }

    protected override void Block()
    {
        base.Block();
    }

    protected GameObject FindTarget()
    {
        BodyPart[] possibleTargets = FindObjectsOfType<BodyPart>();
        List<BodyPart> possibleViableTargets = new List<BodyPart>();

        // TODO: Make this more efficient, just made it worse
        for (int i = 0; i < possibleTargets.Length; i++)
        {
            if (possibleTargets[i].owner.playerNumber != this.owner.playerNumber)
            {
                if ((possibleTargets[i].gameObject.transform.position - this.transform.position).magnitude < aggroDistance && possibleTargets[i].owner.health > 0) 
                {
                    possibleViableTargets.Add(possibleTargets[i]);
                }
            }
        }
        if (possibleViableTargets.Count > 0)
        {
            int index = Random.Range(0, possibleViableTargets.Count);

            return possibleViableTargets[index].gameObject;
        }
        else
        {
            return null;
        }
    }

    public IEnumerator WaitToFindTarget(float time)
    {
        waiting = true;
        yield return new WaitForSeconds(time);
        target = FindTarget();
        waiting = false;
    }

    public void OnDestroy()
    {
        ArenaUIManager arenaUI = FindObjectOfType<ArenaUIManager>();

        if (arenaUI != null)
        {
            arenaUI.EnemyDied();
        }
    }
}