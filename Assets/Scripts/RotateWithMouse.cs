using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    public float val1;
    public float val2;
    private float _lookAngle;
    private Vector2 _lookDirection;
    private PlayerController _playerController;

    // Start is called before the first frame update
    private void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Gets look direction relative to player position based on mouse position
        _lookDirection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        //lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        //Sets rotation to look angle
        transform.rotation = Quaternion.Euler(0f, 0f, _lookAngle);

        if (_playerController.lookingRight) //Checks if the player controller is looking right or not and adjusts lookAngle accordingly
        {
            _lookAngle = val1 * Mathf.Rad2Deg + Mathf.Atan2(_lookDirection.x, -_lookDirection.y) * Mathf.Rad2Deg;
        }
        else
        {
            _lookAngle = val2 * Mathf.Rad2Deg + Mathf.Atan2(-_lookDirection.x, -_lookDirection.y) * Mathf.Rad2Deg;
        }
    }
}
