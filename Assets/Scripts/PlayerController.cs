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
    public float rampSlideThresholdY = 10f;
    public float rampSlideThresholdX = 32f;
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
    public TMP_Text totalVelCounter;

    public float totalVelocity;

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
        #region Calculating total velocity
        totalVelocity = Mathf.Sqrt(rb.velocity.x * rb.velocity.x + rb.velocity.y * rb.velocity.y);
        #endregion

        #region Sprite flipping based on look direction
        //Gets look direction relative to player position based on mouse location on screen
        lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        //Flips player sprite based on look direction
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
        #endregion

        #region Raycast
        //Sets raycast position to the middle of the player collider
        _raycastPosition = rb.position - new Vector2(0, cc.size.y * 0.5f);

        //Calls the DrawDebugRays function
        DrawDebugRays(Vector2.down, Color.green);

        //I don't really know what this does
        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, Vector2.down, raycastDistance, groundLayer);

        //Also don't know what this does
        if (hit.collider)
        {
            _raycastHit = hit;
        }
        #endregion

        #region General horizontal movement & slope handling
        //The next 4 lines of movement code were taken from https://youtu.be/KbtcEVCM7bw
        //Calculates desired move direction and velocity
        float targetSpeed = _input * speed;
        //Calculates difference between current velocity and desired velocity
        float speedDiff = targetSpeed - rb.velocity.x;
        //Changes acceleration rate depending on situation
        float accelRate = Mathf.Abs(targetSpeed) > 0.01f ? acceleration : deceleration;
        //Applies acceleration to speed difference, then raises to a set power so that acceleration increases with higher speeds
        //Finally multiplies by sign to reapply direction
        float movement = Mathf.Pow(Mathf.Abs(speedDiff) * accelRate, velPower) * Mathf.Sign(speedDiff);

        if (IsGrounded() && rb.velocity.y < rampSlideThresholdY && rb.velocity.x < rampSlideThresholdX) //If player is grounded and not rampsliding
        {
            //Gets normal of the slope the player is standing on based on the raycast
            slopeNormal = _raycastHit.normal;
            //slopeAngle = Vector2.SignedAngle(slopeNormal, Vector2.up);
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
            
            // Adds movement forces to Y & X axes, multiplied by slope normals
            rb.AddForce(slopeNormal.y * (movement * Vector2.right));
            rb.AddForce(slopeNormal.x * (movement * Vector2.down));

            //Player is grounded and not rampsliding
            grounded = true;
            rampSliding = false;
        }
        else //If the player is in the air or rampsliding
        {
            //Adds speed * input as a force to X axis (instead of using the movement value defined earlier)
            rb.AddForce(_input * speed * Vector2.right - Vector2.down * gravity);
            //Player is grounded
            grounded = false;
            //If player is grounded, they are rampsliding, otherwise they are not rampsliding
            if (IsGrounded())
            {
                rampSliding = true;
            }
            else
            {
                rampSliding = false;
            }
        }
        #endregion

        #region Friction
        if (rb.velocity.y > rampSlideThresholdY && rb.velocity.x > rampSlideThresholdX) //If player Y & X velocity > ramp slide thresholds
        {
            //Do nothing (friction not applied)
        }
        else
        {
            if (IsGrounded() && !isMoving) //Checks if player is both grounded and not pressing any horizontal movement buttons
            {
                //This code was also taken from https://youtu.be/KbtcEVCM7bw
                //Sets 'amount' to either current velocity or friction amount (whichever is smaller)
                float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
                //Sets it to current movement direction
                amount *= Mathf.Sign(rb.velocity.x);
                //Applies as a force against movement direction
                rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
            }
        }
        #endregion

        #region UI
        //Sets X and Y velocity UI to mirror player velocity
        xVelCounter.text = "xv:" + (int)Mathf.Abs(Mathf.RoundToInt(rb.velocity.x));
        yVelCounter.text = "yv:" + (int)Mathf.Abs(Mathf.RoundToInt(rb.velocity.y));
        totalVelCounter.text = "v:" + (int)Mathf.RoundToInt(totalVelocity);
        #endregion

        #region Coyote Time (Removed)
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
        #endregion
    }

    public bool IsGrounded()
    {
        //Returns true if the groundcheck GameObject at the player's feet is close enough to the ground, using groundLayer as a layermask to determine what layer contains level geometry
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    public void Move(InputAction.CallbackContext context)
    {
        // I don't really get how this works because it's using the new input system which I don't really understand, but it gets user input on the X axis somehow
        _input = context.ReadValue<Vector2>().x;

        if (context.started) //If player is pressing movement buttons, isMoving is true, otherwise false
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
        if (context.started) //If jump button is pressed, jumpBufferCounter resets to match the jumpBufferTime value
        {
            _jumpBufferCounter = jumpBufferTime;
        }
        else //jumpBufferCounter decreases with time
        {
            _jumpBufferCounter -= Time.deltaTime;
        }

        if (_jumpBufferCounter > 0f && IsGrounded()) //Checks if grounded and if jumpBufferCounter > 0
        {
            rb.AddForce(Vector2.up * jumpingPower, ForceMode2D.Impulse); //Applies jump force

            _jumpBufferCounter = 0f; //Sets jumpBufferCounter to zero
        }
    }

    //Draws a raycast
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
