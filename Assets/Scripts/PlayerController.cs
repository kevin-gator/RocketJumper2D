using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;

    private float _input;
    public float speed = 8f;
    public float jumpingPower = 8f;
    public float acceleration = 1f;
    public float deceleration = 1f;
    public float velPower = 1f;
    public float frictionAmount = 1f;
    public float rampSlideThreshold = 15f;
    public float gravity = -40f;
    public float raycastDistance = 0.5f;
    //public float coyoteTime = 0.2f;
    //private float coyoteTimeCounter;
    public float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    public bool isMoving;
    public bool grounded = false;
    public bool onSlope = false;
    
    private Vector2 raycastPosition;
    private RaycastHit2D raycastHit;

    public Vector2 slopeNormal;
    public float slopeAngle;

    public CapsuleCollider2D cc;
    //private Vector2 colliderSize = cc.size;

    public Vector2 lookDirection;
    public bool lookingRight;
    public bool rampSliding;

    private SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        // cc = GetComponent<CapsuleCollider2D>();
        // colliderSize = cc.size;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
        
        lookDirection = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (lookDirection.x >= 0)
        {
            spriteRenderer.flipX = false;
            lookingRight = true;
        }
        else if (lookDirection.x < 0)
        {
            spriteRenderer.flipX = true;
            lookingRight = false;
        }

        raycastPosition = rb.position - new Vector2(0, cc.size.y * 0.5f);

        DrawDebugRays(Vector2.down, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(raycastPosition, Vector2.down, raycastDistance, groundLayer);

        if (hit.collider)
        {
            raycastHit = hit;
        }

        float targetSpeed = _input * speed;

        float speedDiff = targetSpeed - rb.velocity.x;

        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

        if(IsGrounded() && rb.velocity.y < rampSlideThreshold)
        {
            slopeNormal = raycastHit.normal;
            slopeAngle = Vector2.SignedAngle(slopeNormal, Vector2.up);
            /*
            if(slopeAngle != 0)
            {    
                //float adjustedVelocity = -Mathf.Abs(Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * _input);
                //Quaternion Rotation = Quaternion.Euler(0, 0, slopeAngle);
                rb.AddForce(slopeNormal.y * (movement * Vector2.right));
                rb.AddForce(slopeNormal.x * (movement * Vector2.down));

                onSlope = true;
            }
            else if (slopeAngle == 0)
            {
                //Quaternion Rotation = Quaternion.Euler(0, 0, slopeAngle);
                //rb.AddForce(Rotation * (movement * Vector2.right));

                rb.AddForce(movement * Vector2.right);
                onSlope = false;
            }*/

            rb.AddForce(slopeNormal.y * (movement * Vector2.right));
            rb.AddForce(slopeNormal.x * (movement * Vector2.down));

            grounded = true;
            rampSliding = false;
        }
        else
        {
            rb.AddForce(_input * speed * Vector2.right - Vector2.down * gravity);
            grounded = false;
            if(IsGrounded())
            {
                rampSliding = true;
            }
            else
            {
                rampSliding = false;
            }
        }

        //Friction
        if(rb.velocity.y > rampSlideThreshold)
        {

        }
        else
        {
            if (IsGrounded() && !isMoving)
            {
                
                float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));

                amount *= Mathf.Sign(rb.velocity.x);

                rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }
        }

        //Coyote Time
        /*
        if(IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }*/
    }

    public bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>().x;

        if (context.started)
        {
            isMoving = true;
        }
        else if(context.canceled)
        {
            isMoving = false;
        }

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }
        
        if(jumpBufferCounter > 0f && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);

            jumpBufferCounter = 0f;
        }
    }

    private void DrawDebugRays(Vector2 direction, Color color)
    {
        Debug.DrawRay(raycastPosition, direction * raycastDistance, color);
    }

    /*
    private void SlopeCheck()
    {
        Vector2 checkPos = new Vector2(0f, cc.size.y / 2);
        
       SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, groundLayer);
        if(hit)
        {
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }
    }*/
}
