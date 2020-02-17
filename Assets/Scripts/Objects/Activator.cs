using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public List<IEntity> entities;

    public string activateOnButtonPress;

    public bool activateOnTriggerEnter = false;
    public bool activateOnCollisionEnter = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton(activateOnButtonPress))
        {
            Activate();
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (activateOnTriggerEnter)
        {
            Activate();
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (activateOnCollisionEnter)
        {
            Activate();
        }
    }

    public void Activate()
    {
        foreach (var entity in entities)
        {
            entity.active = true;
        }
    }

    public void Deactivate()
    {
        foreach (var entity in entities)
        {
            entity.active = false;
        }
    }
}
