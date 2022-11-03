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

    void Start()
    {
        line = GetComponent<LineRenderer>();
        joint = GetComponent<DistanceJoint2D>();
        line.enabled = false;
        joint.enabled = false;
    }

    
    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Mouse1))
        {
            FireHook();
            line.enabled = true;
        }

        if(Input.GetKeyUp(KeyCode.Mouse1) || distanceFromPlayer > maxDistance)
        {
            line.enabled = false;
            joint.enabled = false;
            distanceFromPlayer = Vector3.Distance(barrelTip.position, transform.position);
        }

        line.SetPosition(0, barrelTip.position);
    }

    private void FireHook()
    {
        GameObject firedGrappleHook = Instantiate(grappleHook, firePoint.position, barrelTip.rotation);
        firedGrappleHook.GetComponent<Rigidbody2D>().velocity = firePoint.right * fireSpeed;
    }
}
