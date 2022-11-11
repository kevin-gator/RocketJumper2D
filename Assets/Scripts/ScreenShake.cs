using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField]
    private float _screenShakeTime = 0.2f;
    [SerializeField]
    private float _screenShakeIntensity = 0.5f;

    private CinemachineVirtualCamera _virtualCamera;
    private float _screenShakeTimer;

    void Start()
    {
        _virtualCamera = gameObject.GetComponent<CinemachineVirtualCamera>();
    }

    void Update()
    {
        if(_screenShakeTimer > 0)
        {
            _screenShakeTimer -= Time.deltaTime;
        }
        else
        {
            _screenShakeTimer = 0;
        }
        
        if(_screenShakeTimer > _screenShakeTime / 2)
        {
            _virtualCamera.m_Lens.OrthographicSize -= _screenShakeIntensity * Time.deltaTime;
        }
        else if(_screenShakeTimer > 0)
        {
            _virtualCamera.m_Lens.OrthographicSize += _screenShakeIntensity * Time.deltaTime;
        }
        else
        {
            
        }

    }

    public void Shake()
    {
        _screenShakeTimer = _screenShakeTime;
    }
}
