using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed;
    public float turnSpeed;
    public float jumpForce;
    public float rotationLockAngle;
    public float hoverHeight = 1;
    public float hoverForce;
    public float tiltAngle = 10.0f;
    public GameObject[] hoverPoints;

    private float rotationY = 0;
    private float rotationZ = 0;
    private Rigidbody rb;
    private bool jumping = false;

    int layerMask;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        layerMask = 1 << LayerMask.NameToLayer("Player");
        layerMask = ~layerMask;
    }

    private void FixedUpdate()
    {
        
        float moveHorizontal = Input.GetAxis("Horizontal");

        //Works, needs to be cleaner
        rb.AddRelativeForce(Vector3.forward * moveSpeed);
        rb.AddRelativeTorque(0, moveHorizontal * turnSpeed, 0);

        if (rb.velocity.magnitude > moveSpeed) //  || rb.velocity.magnitude < moveSpeed
            rb.velocity = rb.velocity.normalized * moveSpeed;

        //Vector3 movement = new Vector3(0.0f, moveHorizontal, 0.0f);  
        //transform.rotation = Quaternion.Euler(0, 0, rb.velocity.x * -tiltAngle);
        
        if(jumping == false)
        {
            if (Input.GetMouseButton(0) || Input.GetButton("Jump"))
            {
                rb.AddForce(0, jumpForce, 0, ForceMode.Impulse);
                jumping = true;
            }
        }
        Hover();
    }

    private void LateUpdate()
    {
        //LockedRotation();
    }

    // needs to be relative to player "forward"
    void LockedRotation()
    {  
        rotationY += Input.GetAxis("Mouse X");
        rotationZ += rb.velocity.x * tiltAngle;
        rotationY = Mathf.Clamp(rotationY, -rotationLockAngle, rotationLockAngle);
        rotationZ = Mathf.Clamp(rotationZ, -tiltAngle, tiltAngle);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -rotationY, -rotationZ);
    }

    void Hover()
    {

        RaycastHit hit;

        for (int i = 0; i < hoverPoints.Length; i++)
        {
            
            var hoverPoint = hoverPoints[i];
            Debug.DrawRay(hoverPoint.transform.position, -transform.up);
            if (Physics.Raycast(hoverPoint.transform.position, -Vector3.up, out hit, hoverHeight, layerMask))
                rb.AddForceAtPosition(Vector3.up * hoverForce * (1.0f - (hit.distance / hoverHeight)), hoverPoint.transform.position);       
            
            /*else
            {
                if (transform.position.y > hoverPoint.transform.position.y)
                    rb.AddForceAtPosition(hoverPoint.transform.up * hoverForce, hoverPoint.transform.position);
                else
                    rb.AddForceAtPosition(hoverPoint.transform.up * -hoverForce, hoverPoint.transform.position);
            }*/
            if (hit.distance > 0.5f)
            {
                jumping = false;
            }

        }


        
        /*Ray ray = new Ray(transform.position, -transform.up);
        RaycastHit hit;

        Debug.DrawRay(transform.position, -transform.up);

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            rb.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }

        if (hit.distance > 0.5f)
        {
            jumping = false;
        }*/
    }

    /*void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Road"))
        {
            jumping = false;
        }
    }*/
}
