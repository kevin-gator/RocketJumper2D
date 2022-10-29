using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerAnimations : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody2D rb;
    private Animator animator;

    public Vector2 lookDirection;
    public GameObject rpg;
    public Barrel rpgBarrel;

    // Start is called before the first frame update
    void Start()
    {
        playerController = gameObject.GetComponent<PlayerController>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        rpgBarrel = rpg.GetComponent<Barrel>();

        lookDirection = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
        animator.SetFloat("horizontalMovement", rb.velocity.x);

        animator.SetFloat("verticalMovement", rb.velocity.y);

        if(playerController.lookingRight)
        {
            animator.SetBool("lookingRight", true);
        }
        else
        {
            animator.SetBool("lookingRight", false);
        }

        if(playerController.IsGrounded())
        {
            animator.SetBool("isGrounded", true);
        }
        else
        {
            animator.SetBool("isGrounded", false);
        }
    }
}
