using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateRArm : MonoBehaviour
{
    private Vector2 lookDirection;
    private float lookAngle;
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        lookDirection = UnityEngine.Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, lookAngle);
        
        if(playerController.lookingRight)
        {
            lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        }
        else
        {
            lookAngle = Mathf.Atan2(lookDirection.y, -lookDirection.x) * Mathf.Rad2Deg;
        }
    }
}
