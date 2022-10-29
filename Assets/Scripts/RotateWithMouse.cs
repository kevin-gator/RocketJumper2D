using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    private Vector2 lookDirection;
    private float lookAngle;
    private PlayerController playerController;
    public float val1;
    public float val2;

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
            lookAngle = (val1 * Mathf.Rad2Deg) + Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg;
        }
        else
        {
            lookAngle = (val2 * Mathf.Rad2Deg) + Mathf.Atan2(-lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg;
        }
    }
}
