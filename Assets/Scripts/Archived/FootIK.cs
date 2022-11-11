using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootIK : MonoBehaviour
{
    public float raycastDistance = 0.25f;
    public LayerMask groundLayer;
    private RaycastHit2D _raycastHit;
    public Transform footTarget;
    private Vector2 _raycastPosition;
    public PlayerController playerController;
    public Transform playerPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void LateUpdate()
    {
        _raycastPosition = transform.position;
        
        DrawDebugRays(Vector2.down, Color.green);

        RaycastHit2D hit = Physics2D.Raycast(_raycastPosition, Vector2.down, raycastDistance, groundLayer); ;

        if (hit.collider)
        {
            _raycastHit = hit;
            footTarget.position = new Vector3(footTarget.position.x, hit.point.y);
        }
        
    }

    private void DrawDebugRays(Vector2 direction, Color color)
    {
        Debug.DrawRay(_raycastPosition, direction * raycastDistance, color);
    }
}