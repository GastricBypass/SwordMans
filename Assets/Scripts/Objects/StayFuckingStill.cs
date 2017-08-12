using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StayFuckingStill : MonoBehaviour {

    public bool xPos;
    public bool yPos;
    public bool zPos;

    public bool xRot;
    public bool yRot;
    public bool zRot;

    private Rigidbody rigbod;
    private Vector3 startPosition;
    private Vector3 startRotation;

	// Use this for initialization
	void Start ()
    {
        rigbod = this.GetComponent<Rigidbody>();

        startPosition = transform.position;
        startRotation = new Vector3(transform.rotation.x, transform.rotation.y, transform.rotation.z);
	}
	
	// Update is called once per frame
	void Update ()
    {
        float posX = startPosition.x;
        float posY = startPosition.y;
        float posZ = startPosition.z;

        float rotX = startRotation.x;
        float rotY = startRotation.y;
        float rotZ = startRotation.z;

        if (!xPos) posX = rigbod.position.x;
        if (!yPos) posY = rigbod.position.y;
        if (!zPos) posZ = rigbod.position.z;

        if (!xRot) rotX = rigbod.rotation.x;
        if (!yRot) rotY = rigbod.rotation.y;
        if (!zRot) rotZ = rigbod.rotation.z;

        rigbod.position = new Vector3(posX, posY, posZ);
        rigbod.rotation = Quaternion.Euler(new Vector3(rotX, rotY, rotZ));
    }
}
