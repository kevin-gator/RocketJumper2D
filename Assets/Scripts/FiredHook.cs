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
        _grapplingHookScript.line.SetPosition(1, transform.position);

        _grapplingHookScript.joint.connectedAnchor = transform.position;

        _grapplingHookScript.distanceFromPlayer = Vector3.Distance(_player.transform.position, transform.position);

        _rotateDirection = _player.transform.position - transform.position;

        _rotateAngle = Mathf.Atan2(_rotateDirection.x, -_rotateDirection.y) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, _rotateAngle + 90);

        if (Input.GetKeyUp(KeyCode.Mouse1) || _grapplingHookScript.distanceFromPlayer > _grapplingHookScript.maxDistance)
        {
            Destroy(gameObject);
        }
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag != "NoGrapple" && other.gameObject.layer != 3)
        {
            _rb.velocity = Vector3.zero;
            _grapplingHookScript.joint.enabled = true;
            Debug.Log("Grappled!");
        }
        else if(other.gameObject.tag == "NoGrapple" && other.gameObject.layer != 3)
        {
            Destroy(gameObject);
        }
    }
}
