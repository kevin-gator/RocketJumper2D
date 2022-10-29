using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleMaterial : MonoBehaviour
{
    public SpringJoint2D springJoint;
    private Rigidbody2D rb;
    
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseOver()
    {
        if(Input.GetMouseButtonDown(1))
        {
            springJoint.connectedBody = rb;
        }
    }
}
