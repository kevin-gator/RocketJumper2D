using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField]
    private Transform barrelTip;

    [SerializeField]
    private GameObject bullet;

    public GameObject muzzleFlash;
    private MuzzleFlash muzzleFlashScript;

    private Vector2 lookDirection;
    private float lookAngle;

    public float fireSpeed = 20f;

    public float fireRate = 0.25f;
    private float lastFireTime;

    public GameObject player;
    
    private PlayerController playerController;
    private Animator playerAnimator;

    private void Start()
    {
        playerController = player.GetComponent<PlayerController>();
        playerAnimator = player.GetComponent<Animator>();
        muzzleFlashScript = muzzleFlash.GetComponent<MuzzleFlash>();
    }

    // Update is called once per frame
    void Update()
    {
        lookDirection = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        
        /*if (lookDirection.x >= 0)
        {
        lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        }
        else if (lookDirection.x < 0)
        {
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        }*/
        
        if (Input.GetMouseButton(0) && Time.time > lastFireTime + fireRate)
        {
            FireBullet();
            lastFireTime = Time.time;
            muzzleFlashScript.ActivateMuzzleFlash();
        }
    }

    private void FireBullet()
    {
        GameObject firedBullet = Instantiate(bullet, barrelTip.position, barrelTip.rotation);
        firedBullet.GetComponent<Rigidbody2D>().velocity = barrelTip.right * fireSpeed;

        playerAnimator.SetTrigger("fireGun");
        
        /*
        if (playerController.lookingRight)
        {
            firedBullet.GetComponent<Rigidbody2D>().velocity = barrelTip.right * fireSpeed;
        }
        else
        {
            firedBullet.GetComponent<Rigidbody2D>().velocity = barrelTip.right * fireSpeed;
        }*/
    }
}
