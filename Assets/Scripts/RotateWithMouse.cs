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
    public float aimAssistAmount;
    public LayerMask grappleLayer;
    public bool aimingAtGrapplePoint;


    // Start is called before the first frame update
    private void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Gets look direction based on the difference between spine position and _lookPoint
        lookDirection = _lookPoint - transform.position;

        //If RMB is not being held down
        if (!Input.GetMouseButton(1))
        {
            //Sets _lookPoint to the mouse position
            _lookPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        //If RMB is being held down
        else
        {
            AimAssist();
            SetLookPointToAnchor();
        }


        //lookAngle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        //Sets rotation to look angle
        //transform.localRotation = Quaternion.Euler(0f, 0f, (_lookAngle - 90) * 0.95);

        //Checks if the player controller is looking right or not and adjusts lookAngle and localRotation accordingly
        if (_playerController.lookingRight)
        {
            _lookAngle = Mathf.Atan2(lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0f, 0f, (_lookAngle - 90) * 0.95f);
        }
        else
        {
            _lookAngle = Mathf.Atan2(-lookDirection.x, -lookDirection.y) * Mathf.Rad2Deg;
            transform.localRotation = Quaternion.Euler(0f, 0f, (_lookAngle - 90) * 1.05f);
        }
        if (Physics2D.OverlapCircle(Camera.main.ScreenToWorldPoint(Input.mousePosition), aimAssistAmount, grappleLayer))
        {
            aimingAtGrapplePoint = true;
        }
        else
        {
            aimingAtGrapplePoint = false;
        }
    }

    private void AimAssist()
    {
        RaycastHit2D hit = Physics2D.CircleCast(Camera.main.ScreenToWorldPoint(Input.mousePosition), aimAssistAmount, Vector2.zero, Mathf.Infinity, grappleLayer);
        if (aimingAtGrapplePoint == true)
        {
            _lookPoint = hit.point;
        }
        else
        {
            _lookPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }
    
    async private void SetLookPointToAnchor()
    {
        //Tiny delay to avoid issues
        await Task.Delay(2);
        //Sets _lookPoint to the position of the fired grappling hook
        _lookPoint = GameObject.Find("grappleHook (Clone)").transform.position;
    }
}
