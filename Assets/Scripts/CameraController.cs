using Cinemachine;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public Transform playerPosition;
    public float min = 5f; //Minimum player velocity to start effecting camera scale
    public float max = 100f; //Maximum player velocity to stop effecting camera scale
    public float scalingValue = 0.5f;
    public float adjustSpeed = 0.8f;
    private PlayerController _playerController;
    private Rigidbody2D _rb;
    private float _targetSize;
    private CinemachineVirtualCamera _virtualCamera;
    private float _xInput;
    private float _yInput;
    public float movementSpeed = 1f;
    public float zoomSpeed = 1f;
    public float minZoomSize = 5f;
    public float maxZoomSize = 200f;

    // Start is called before the first frame update
    private void Start()
    {
        _playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        _rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        _virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    private void Update()
    {
        if(!Input.GetKey(KeyCode.LeftShift)) //Disables this when holding shift
        {
            //Handles scaling of camera based on player velocity
            if (Mathf.Abs(_rb.velocity.x) <= min && Mathf.Abs(_rb.velocity.y) <= min)
            {
                _targetSize = min * scalingValue;
            }
            else if (Mathf.Abs(_rb.velocity.x) >= max || Mathf.Abs(_rb.velocity.y) >= max)
            {
                _targetSize = max * scalingValue;
            }
            else
            {
                if (Mathf.Abs(_rb.velocity.x) > Mathf.Abs(_rb.velocity.y))
                {
                    _targetSize = Mathf.Abs(_rb.velocity.x) * scalingValue;
                }
                else if (Mathf.Abs(_rb.velocity.y) >= Mathf.Abs(_rb.velocity.x))
                {
                    _targetSize = Mathf.Abs(_rb.velocity.y) * scalingValue;
                }
            }

            //Adds smooth transitions between different camera scales
            float sizeDifference = _targetSize - _virtualCamera.m_Lens.OrthographicSize;
            if (sizeDifference > 0)
            {
                _virtualCamera.m_Lens.OrthographicSize += (adjustSpeed + Mathf.Abs(sizeDifference)) * Time.deltaTime;
            }
            else if (sizeDifference < 0)
            {
                _virtualCamera.m_Lens.OrthographicSize -= (adjustSpeed + Mathf.Abs(sizeDifference)) * Time.deltaTime;
            }
        }
        //Disables following the player when holding down shift
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _virtualCamera.Follow = null;
        }
        else
        {
            _virtualCamera.Follow = playerPosition;
        }
        
        //Allows camera movement with WASD
        transform.position = transform.position + new Vector3(_xInput * movementSpeed * Time.deltaTime, _yInput * movementSpeed * Time.deltaTime);

        //Allows zooming camera
        if(Input.GetKey(KeyCode.LeftShift))
        {
            float zoomAmount = zoomSpeed * -Input.mouseScrollDelta.y * zoomSpeed;


            if(_virtualCamera.m_Lens.OrthographicSize > maxZoomSize)
            {
                if(zoomAmount > 0)
                {
                    zoomAmount = 0;
                }
            }
            else if(_virtualCamera.m_Lens.OrthographicSize < minZoomSize)
            {
                if (zoomAmount < 0)
                {
                    zoomAmount = 0;
                }
            }
            
            _virtualCamera.m_Lens.OrthographicSize += zoomAmount;
        }

    }

    public void MoveCamera(InputAction.CallbackContext context)
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _xInput = context.ReadValue<Vector2>().x;
            _yInput = context.ReadValue<Vector2>().y;
        }
        else
        {
            _xInput = 0f;
            _yInput = 0f;
        }
    }
}
