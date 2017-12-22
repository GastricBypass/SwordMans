using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearTrap : ITrap
{
    public List<Rigidbody> trapTeeth;
    public Rigidbody hinge;
    public Vector3 biteForce;

    public void FixedUpdate()
    {
        if (!isReady)
        {
            foreach (Rigidbody tooth in trapTeeth)
            {
                tooth.AddForce(biteForce);
            }
            hinge.AddForce(-1 * biteForce * trapTeeth.Count);
        }
    }

    public override void OnTriggerEnter(Collider other)
    {
        if (!trapTeeth.Contains(other.GetComponent<Rigidbody>()))
        {
            base.OnTriggerEnter(other);
        }
    }

    public override void Activate()
    {
        base.Activate();

        foreach (Rigidbody tooth in trapTeeth)
        {
            StickyObject sticky = tooth.gameObject.GetComponent<StickyObject>();
            if (sticky != null)
            {
                sticky.active = true;
            }
        }
    }

    public override void Ready()
    {
        base.Ready();

        foreach (Rigidbody tooth in trapTeeth)
        {
            StickyObject sticky = tooth.gameObject.GetComponent<StickyObject>();
            if (sticky != null)
            {
                sticky.active = false;
            }
        }
    }
}
