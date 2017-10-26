using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : IEntity
{
    public bool forceBased = true;
    public Vector3 spinTorque;
    private Rigidbody rigbod; 
	// Use this for initialization
	void Start () {
        rigbod = this.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (active)
        {
            if (forceBased)
            {
                rigbod.AddTorque(spinTorque);
            }

            else if (rigbod != null)
            {
                rigbod.angularVelocity = spinTorque;
            }

            else
            {
                this.transform.rotation = Quaternion.Euler(this.transform.rotation.eulerAngles + spinTorque * Time.timeScale);
            }
        }
	}
}
