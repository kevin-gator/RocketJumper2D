using System.Threading.Tasks;
using UnityEngine;

public class RotateWithMouse : MonoBehaviour
{
    //public float val1;
    //public float val2;
    //[SerializeField]
    private float _lookAngle;
    public Vector2 lookDirection;
    private PlayerController _playerController;
    private Vector3 _lookPoint;


    // Start is called before the first frame update
    private void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Gets look direction relative to player position based on mouse position
        lookDirection = _lookPoint - transform.position;

        if (!Input.GetMouseButton(1))
        {
            _lookPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        else
        {
            SetLookPointToAnchor();
        }


        //lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        //Sets rotation to look angle
        //transform.localRotation = Quaternion.Euler(0f, 0f, (_lookAngle - 90) * 0.95);

        if (_playerController.lookingRight) //Checks if the player controller is looking right or not and adjusts lookAngle and localRotation accordingly
        {
            _lookAngle = Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0f, 0f, (_lookAngle - 90) * 0.95f);
        }
        else
        {
            _lookAngle = Mathf.Atan2(-lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0f, 0f, (_lookAngle - 90) * 1.05f);
        }
    }

    async private void SetLookPointToAnchor()
    {
        await Task.Delay(10);
        _lookPoint = GameObject.Find("grappleHook (Clone)").transform.position;
    }
}
