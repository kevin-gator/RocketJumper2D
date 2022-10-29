using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    private PlayerController playerController;
    private Rigidbody2D rb;
    private CinemachineVirtualCamera virtualCamera;
    private float targetSize;

    public float min = 5f;
    public float max = 100f;
    public float scalingValue = 0.5f;
    public float adjustSpeed = 0.8f;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Mathf.Abs(rb.velocity.x) <= min && rb.velocity.y <= min)
        {
            targetSize = min * scalingValue;
        }
        else if (Mathf.Abs(rb.velocity.x) >= max || rb.velocity.y >= max)
        {
            targetSize = max * scalingValue;
        }
        else
        {
            if(Mathf.Abs(rb.velocity.x) > rb.velocity.y)
            {
                targetSize = Mathf.Abs(rb.velocity.x) * scalingValue;
            }
            else if(rb.velocity.y >= Mathf.Abs(rb.velocity.x))
            {
                targetSize = rb.velocity.y * scalingValue;
            }
        }

        float sizeDifference = targetSize - virtualCamera.m_Lens.OrthographicSize;
        if(sizeDifference > 0)
        {
            virtualCamera.m_Lens.OrthographicSize += (adjustSpeed + Mathf.Abs(sizeDifference)) * Time.deltaTime;
        }
        else if(sizeDifference < 0)
        {
            virtualCamera.m_Lens.OrthographicSize -= (adjustSpeed + Mathf.Abs(sizeDifference)) * Time.deltaTime;
        }
    }
}
