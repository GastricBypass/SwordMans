using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViscousArea : MonoBehaviour {

    public float dragIncrease = 1;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Rigidbody rigbod = other.GetComponent<Rigidbody>();

        if (rigbod != null)
        {
            rigbod.drag = rigbod.drag + dragIncrease;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Rigidbody rigbod = other.GetComponent<Rigidbody>();

        if (rigbod != null)
        {
            rigbod.drag = rigbod.drag / dragIncrease;
        }
    }
}
