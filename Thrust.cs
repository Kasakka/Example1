using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilt : MonoBehaviour {

    public float tiltAngle = 10.0f;
    private Rigidbody rb;     

	// Use this for initialization
	void Start () {
        rb = GetComponentInChildren<Rigidbody>();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Euler(0, 0, rb.velocity.x * -tiltAngle);
    }
}
