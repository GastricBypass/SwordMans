using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Springboard : IEntity
{
    public bool forceBased = true; 
    public float springIntervalMS;
    public Vector3 springForce;

    private Rigidbody rigbod;
    private System.DateTime lastSpring;

	// Use this for initialization
	void Start () {
        rigbod = this.GetComponent<Rigidbody>();
        lastSpring = System.DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {
		if (active && (System.DateTime.Now - lastSpring).TotalMilliseconds > springIntervalMS)
        {
            Spring();
        }
	}

    void Spring()
    {
        if (forceBased)
        {
            rigbod.AddForce(springForce);
        }
        else
        {
            rigbod.velocity = springForce;
        }
        lastSpring = System.DateTime.Now;
    }
}
