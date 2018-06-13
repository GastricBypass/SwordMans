using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionToggler : MonoBehaviour
{
    public IEntity target;

    public bool enterTurnOff = true;
    public bool enterTurnOn = true;
    public bool exitTurnOff = false;
    public bool exitTurnOn = false;
    
    void Start()
    {
        if (target == null)
        {
            target = this.GetComponent<IEntity>();
        }
        if (target == null)
        {
            Destroy(this.gameObject);
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        TurnOnOrOff(enterTurnOff, enterTurnOn);
    }

    public void OnCollisionExit(Collision collision)
    {
        TurnOnOrOff(exitTurnOff, exitTurnOn);
    }

    public void OnTriggerEnter(Collider other)
    {
        TurnOnOrOff(enterTurnOff, enterTurnOn);
    }

    public void OnTriggerExit(Collider other)
    {
        TurnOnOrOff(exitTurnOff, exitTurnOn);
    }

    private void TurnOnOrOff(bool offCondition, bool onCondition)
    {
        if (target.active && offCondition)
        {
            target.active = false;
        }
        else if (!target.active && onCondition)
        {
            target.active = true;
        }
    }
}

