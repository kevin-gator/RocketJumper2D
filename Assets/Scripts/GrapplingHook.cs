using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour
{
    public GameObject grappleHook;
    public LineRenderer line;
    public DistanceJoint2D joint;

    public Transform barrelTip;
    public Transform firePoint;
    public float fireSpeed = 80f;

    public float distanceFromPlayer = 0f;
    public float maxDistance = 50f;

    public Animator playerAnimator;
    void Start()
    {
        line = GetComponent<LineRenderer>();
        joint = GetComponent<DistanceJoint2D>();
        line.enabled = false;
        joint.enabled = false;
    }

    
    void Update()
    {
        //If RMB is held down
        if (Input.GetKeyDown(KeyCode.Mouse1)) 
        {
            FireHook();
            line.enabled = true;
        }
        //When RMB is released or when the max grapple distance is reached
        if (Input.GetKeyUp(KeyCode.Mouse1) || distanceFromPlayer > maxDistance)
        {
            line.enabled = false;
            joint.enabled = false;
            //Resets max grapple distance
            distanceFromPlayer = Vector3.Distance(barrelTip.position, transform.position); 
        }
        //Sets the first position of the line renderer to the barrel tip
        line.SetPosition(0, barrelTip.position); 
    }

    private void FireHook()
    {
        //Creates a copy of the grappleHook prefab
        GameObject firedGrappleHook = Instantiate(grappleHook, firePoint.position, barrelTip.rotation);
        //Sets the grappleHook copy velocity
        firedGrappleHook.GetComponent<Rigidbody2D>().velocity = firePoint.right * fireSpeed;
        //Triggers the "fireGun" trigger on the player's Animator component
        playerAnimator.SetTrigger("fireGun");
    }
}
