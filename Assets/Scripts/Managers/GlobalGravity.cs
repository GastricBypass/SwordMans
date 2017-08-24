using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGravity : MonoBehaviour {
    
    public float gravityScale = 1;

    private Vector3 oldGravity;

	// Use this for initialization
	void Start ()
    {
        oldGravity = Physics.gravity;
        Physics.gravity = Physics.gravity * gravityScale; // THIS AFFECTS THE GRAVITY ACROSS ALL SCENES!!!
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnDestroy()
    {
        Physics.gravity = oldGravity;
    }
}
