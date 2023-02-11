using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFlicker : MonoBehaviour
{
    private Light2D _light;
    private float _flickerModifier;

    public float lightIntensity = 1.2f;
    public float flickerRange;
    public float flickerSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _light = GetComponent<Light2D>();
        _light.intensity = lightIntensity;
        _flickerModifier = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_light.intensity < lightIntensity - flickerRange)
        {
            _flickerModifier = 1f;
        }
        else if (_light.intensity > lightIntensity + flickerRange)
        {
            _flickerModifier = -1f;
        }

        _light.intensity += Time.deltaTime * flickerSpeed * _flickerModifier;
    }
}
