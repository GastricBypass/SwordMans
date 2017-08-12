using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepUpright : IEntity {

    public float pullStrength;

    private Rigidbody rigbod;

	// Use this for initialization
	void Start () {
        rigbod = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        if (active)
        {
            Vector3 tug = Vector3.up * -pullStrength;

            rigbod.AddForceAtPosition(transform.TransformPoint(Vector3.up), tug);
            rigbod.AddForceAtPosition(transform.TransformPoint(-Vector3.up), -tug);
        }
    }
}
