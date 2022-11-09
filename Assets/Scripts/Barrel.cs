using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField]
    private Transform barrelTip;

    [SerializeField]
    private GameObject bullet;

    public GameObject muzzleFlash;

    public float fireSpeed = 20f;

    public float fireRate = 0.25f;

    public GameObject player;
    private float _lastFireTime;
    private float _lookAngle;

    private Vector2 _lookDirection;
    private MuzzleFlash _muzzleFlashScript;
    private Animator _playerAnimator;

    private PlayerController _playerController;

    public GameObject spine1;

    //[SerializeField]
    //private float adjustValue;
    //public float adjustValueLookingRight = 0.95f;
    //public float adjustValueLookingLeft = 1f;

    //public float val1;

    private void Start()
    {
        _playerController = player.GetComponent<PlayerController>();
        _playerAnimator = player.GetComponent<Animator>();
        _muzzleFlashScript = muzzleFlash.GetComponent<MuzzleFlash>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Gets look direction relative to player position from mouse position
        //_lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //Converts look direction to an angle
        //_lookAngle = Mathf.Atan2(_lookDirection.y, _lookDirection.x) * Mathf.Rad2Deg;
        //Sets the barrel rotation to the look angle
        //transform.rotation = Quaternion.Euler(0f, 0f, _lookAngle * adjustValue);

        //Adjusts rotation based on look direction
        if (_playerController.lookingRight)
        {
            transform.rotation = spine1.transform.rotation * Quaternion.Euler(0, 0, -90);
        }
        else
        {
            transform.rotation = Quaternion.Inverse(spine1.transform.rotation) * Quaternion.Euler(0, 0, -90);
        }

        //Checks if mouse1 is being held down and accounts for firerate
        if (Input.GetMouseButton(0) && Time.time > _lastFireTime + fireRate && !Input.GetMouseButton(1))
        {
            FireBullet();
            //Resets lastFireTime to current time
            _lastFireTime = Time.time;
            //Calls a function in the muzzleFlashScript
            _muzzleFlashScript.ActivateMuzzleFlash();
        }
    }

    private void FireBullet()
    {
        //Instantiates the rocket ("bullet") at the barrelTip
        GameObject firedBullet = Instantiate(bullet, barrelTip.position, barrelTip.rotation);
        //Sets the velocity of the rocket to the barrel direction * fire speed
        firedBullet.GetComponent<Rigidbody2D>().velocity = barrelTip.right * fireSpeed;

        //Triggers the "fireGun" trigger on the player's Animator component
        _playerAnimator.SetTrigger("fireGun");

        /*
        if (_playerController.lookingRight)
        {
            firedBullet.GetComponent<Rigidbody2D>().velocity = barrelTip.right * fireSpeed;
        }
        else
        {
            firedBullet.GetComponent<Rigidbody2D>().velocity = barrelTip.right * fireSpeed;
        }*/
    }
}
