using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float min = 5f;
    public float max = 100f;
    public float scalingValue = 0.5f;
    public float adjustSpeed = 0.8f;
    private PlayerController _playerController;
    private Rigidbody2D _rb;
    private float _targetSize;
    private CinemachineVirtualCamera _virtualCamera;

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
        if (Mathf.Abs(_rb.velocity.x) <= min && _rb.velocity.y <= min)
        {
            _targetSize = min * scalingValue;
        }
        else if (Mathf.Abs(_rb.velocity.x) >= max || _rb.velocity.y >= max)
        {
            _targetSize = max * scalingValue;
        }
        else
        {
            if (Mathf.Abs(_rb.velocity.x) > _rb.velocity.y)
            {
                _targetSize = Mathf.Abs(_rb.velocity.x) * scalingValue;
            }
            else if (_rb.velocity.y >= Mathf.Abs(_rb.velocity.x))
            {
                _targetSize = _rb.velocity.y * scalingValue;
            }
        }

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
}
