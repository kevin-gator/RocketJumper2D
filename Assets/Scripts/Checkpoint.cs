using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Checkpoint : MonoBehaviour
{
    private Animator _animator;
    private bool _active;
    private Light2D _light;
    private float _flickerModifier;

    public float lightIntensity = 1.2f;
    public float flickerRange;
    public float flickerSpeed;
    public float yOffset;
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _active = false;
        _light = GetComponent<Light2D>();
        _light.enabled = false;
        _light.intensity = lightIntensity;
        _flickerModifier = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_active == true)
        {
            _animator.SetBool("isActive", true);
            if (_light.intensity < lightIntensity - flickerRange)
            {
                _flickerModifier = 1f;
            }
            else if(_light.intensity > lightIntensity + flickerRange)
            {
                _flickerModifier = -1f;
            }

            _light.intensity += Time.deltaTime * flickerSpeed * _flickerModifier;
        }
        else
        {
            _animator.SetBool("isActive", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.name == "Player")
        {
            SpawnHandling spawnHandling = collision.gameObject.GetComponent<SpawnHandling>();
            spawnHandling.SetSpawnPosition(new Vector2(transform.position.x, transform.position.y + yOffset));
            _active = true;
            _light.enabled = true;
        }
    }
}
