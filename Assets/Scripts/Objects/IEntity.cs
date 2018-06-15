using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class IEntity : MonoBehaviour
{
    public bool active = true;

    public virtual void Activate()
    {
        active = true;
    }

    public virtual void Deactivate()
    {
        active = false;
    }
}
