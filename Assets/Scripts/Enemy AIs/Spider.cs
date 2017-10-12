using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spider : AIEnemy {

    float hoverHeight = 2;
    float hoverForce = 5;
    float hoverDampening = 0.5f;

    // Use this for initialization
    protected override void Start ()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Move()
    {
        base.Move();

        RaycastHit hit;
        Ray downRay = new Ray(transform.position, -Vector3.up);
        if (Physics.Raycast(downRay, out hit))
        {
            float hoverError = hoverHeight - hit.distance;
            if (hoverError > 0)
            {
                float upwardSpeed = rigbod.velocity.y;
                float lift = hoverError * hoverForce - upwardSpeed * hoverDampening;
                rigbod.AddForce(lift * Vector3.up);
            }
        }
    }
}
