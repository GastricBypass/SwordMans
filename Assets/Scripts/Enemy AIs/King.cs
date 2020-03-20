using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Man
{
    public GameObject sword;

    protected override void ExtraDeathEffects()
    {
        Joint[] joints = sword.gameObject.GetComponents<Joint>();
        foreach (Joint joint in joints)
        {
            Destroy(joint);
        }

        Collider[] colliders = sword.gameObject.GetComponents<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.material = null; // Make weapons have friction after the person dies.
        }

        for (int i = 0; i < sword.transform.childCount; i++)
        {
            Collider collider = sword.transform.GetChild(i).GetComponent<Collider>();
            if (collider != null)
            {
                collider.material = null; // Make the weapons child colliders also have friction.
            }
        }

        var path = sword.gameObject.GetComponent<PathFollower>();
        if (path != null)
        {
            Destroy(path);
        }

        ForcePlayersLose();
    }

    public override bool CanUpdateUiHealth()
    {
        return false;
    }

    private void ForcePlayersLose()
    {
        var ui = FindObjectOfType<CampaignUIManager>();

        ui.ForcePlayerLose();
    }
}
