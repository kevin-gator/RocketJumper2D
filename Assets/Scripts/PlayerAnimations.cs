using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{

    public GameObject rpg;
    public Barrel rpgBarrel;
    private Animator _animator;
    private PlayerController _playerController;
    private Rigidbody2D _rb;
    public GameObject spine_1;

    // Start is called before the first frame update
    private void Start()
    {
        _playerController = gameObject.GetComponent<PlayerController>();
        _rb = gameObject.GetComponent<Rigidbody2D>();
        _animator = gameObject.GetComponent<Animator>();
        rpgBarrel = rpg.GetComponent<Barrel>();
    }

    // Update is called once per frame
    private void Update()
    {

        _animator.SetFloat("horizontalMovement", _rb.velocity.x);

        _animator.SetFloat("verticalMovement", _rb.velocity.y);

        if (_playerController.lookingRight)
        {
            _animator.SetBool("lookingRight", true);
        }
        else
        {
            _animator.SetBool("lookingRight", false);
        }

        if (_playerController.IsGrounded())
        {
            _animator.SetBool("isGrounded", true);
        }
        else
        {
            _animator.SetBool("isGrounded", false);
        }

        if(_playerController.rampSliding)
        {
            _animator.SetBool("rampSliding", true);
        }
        else
        {
            _animator.SetBool("rampSliding", false);
        }

        if(_playerController.isMoving)
        {
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }


    }
}
