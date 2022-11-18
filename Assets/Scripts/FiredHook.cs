using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiredHook : MonoBehaviour
{
    private GameObject _player;
    private GrapplingHook _grapplingHookScript;
    private Rigidbody2D _rb;
    private Vector2 _rotateDirection;
    private float _rotateAngle;
    public LayerMask groundLayer;

    void Start()
    {
        _player = GameObject.Find("Player");
        _grapplingHookScript = _player.GetComponent<GrapplingHook>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        //Sets the second position of the line renderer to the position of the grappling hook
        _grapplingHookScript.line.SetPosition(1, transform.position);
        //Sets the anchor point of the distance joint to the position of the grappling hook
        _grapplingHookScript.joint.connectedAnchor = transform.position;
        //Calculates distance between the player and the grappling hook
        _grapplingHookScript.distanceFromPlayer = Vector3.Distance(_player.transform.position, transform.position);
        //Gets the position of the grapple hook relative to the player as a Vector2
        _rotateDirection = _player.transform.position - transform.position;
        //Converts the rotate direction to an angle in degrees
        _rotateAngle = Mathf.Atan2(_rotateDirection.x, -_rotateDirection.y) * Mathf.Rad2Deg;
        //Rotates the grappling hook to face the player's current position
        transform.rotation = Quaternion.Euler(0, 0, _rotateAngle + 90);
        //Destroys the grappling hook when RMB is released or the max fire distance is reached
        if (Input.GetKeyUp(KeyCode.Mouse1) || _grapplingHookScript.distanceFromPlayer > _grapplingHookScript.maxDistance)
        {
            Destroy(gameObject);
        }
    }
    
    //Triggered when the grappling hook hits a collider
    public void OnTriggerEnter2D(Collider2D other)
    {
        Material_Grappleable grappleable = other.gameObject.GetComponent<Material_Grappleable>();

        //If the collider gameObject is grappleable and not on the player layer
        if (grappleable != null && other.gameObject.layer != 3)
        {
            //Stops the grappling hook movement
            _rb.velocity = Vector3.zero;
            //Turns on the distance joint
            _grapplingHookScript.joint.enabled = true;
            //Debug.Log("Grappled!");
        }
        //If the collider gameObject is not grappleable and not on the player layer, destroy the grappling hook and line renderer
        else if(other.gameObject.layer != 3)
        {
            _grapplingHookScript.line.enabled = false;
            Destroy(gameObject);
        }
    }
}
