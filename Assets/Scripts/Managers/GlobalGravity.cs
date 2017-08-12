using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGravity : MonoBehaviour {

    public bool useGravity = true;
    public float gravityScale = 1;

	// Use this for initialization
	void Start () {
        Rigidbody[] rigbods = FindObjectsOfType<Rigidbody>();

        for (int i = 0; i < rigbods.Length; i++)
        {
            rigbods[i].useGravity = this.useGravity;
            if (this.useGravity)
            {
                rigbods[i].drag = rigbods[i].drag / gravityScale;
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
