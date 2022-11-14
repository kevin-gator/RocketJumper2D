using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnHandling : MonoBehaviour
{
    public Vector3 spawnPosition;
    private Rigidbody2D _rb;
    
    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSpawnPosition(Vector3 pos)
    {
        spawnPosition = pos;
    }

    public void Respawn()
    {
        transform.position = spawnPosition;
        _rb.velocity = Vector3.zero;
    }
}
