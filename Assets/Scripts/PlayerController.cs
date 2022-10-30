using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;
    public Transform groundCheck;
    public LayerMask groundLayer;
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

    public bool isMoving;
    public bool grounded;
    public bool onSlope;

    public Vector2 slopeNormal;
    public float slopeAngle;

    public CapsuleCollider2D cc;
    //private Vector2 colliderSize = cc.size;

    public Vector2 lookDirection;
    public bool lookingRight;
    public bool rampSliding;

    private float _input;
    private float _jumpBufferCounter;
    private RaycastHit2D _raycastHit;

    private Vector2 _raycastPosition;

    private SpriteRenderer _spriteRenderer;

    public TMP_Text xVelCounter;
    public TMP_Text yVelCounter;

    // Start is called before the first frame update
    private void Start()
    {
        // rb = GetComponent<Rigidbody2D>();
        // cc = GetComponent<CapsuleCollider2D>();
        // colliderSize = cc.size;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        if (lookDirection.x >= 0)
        {
            _spriteRenderer.flipX = false;
            lookingRight = true;
        }
        else if (lookDirection.x < 0)
        {
            _spriteRenderer.flipX = true;
            lookingRight = false;
        }

        _raycastPosition = rb.position - new Vector2(0, cc.size.y * 0.5f);

        DrawDebugRays(Vector2.down, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, Vector2.down, raycastDistance, groundLayer);

        if (hit.collider)
        {
            _raycastHit = hit;
        }

        float targetSpeed = _input * speed;

        float speedDiff = targetSpeed - rb.velocity.x;

        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;

        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

        if (IsGrounded() && rb.velocity.y < rampSlideThreshold)
        {
            slopeNormal = _raycastHit.normal;
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
            if (IsGrounded())
            {
                rampSliding = true;
            }
            else
            {
                rampSliding = false;
            }
        }

        //Friction
        if (rb.velocity.y > rampSlideThreshold)
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

        xVelCounter.text = "xv:" + (int)Mathf.Abs(Mathf.RoundToInt(rb.velocity.x));
        yVelCounter.text = "yv:" + (int)Mathf.Abs(Mathf.RoundToInt(rb.velocity.y));

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
        else if (context.canceled)
        {
            isMoving = false;
        }

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0f && IsGrounded())
        {
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse);

            _jumpBufferCounter = 0f;
        }
    }

    private void DrawDebugRays(Vector2 direction, Color color)
    {
        Debug.DrawRay(_raycastPosition, direction * raycastDistance, color);
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
