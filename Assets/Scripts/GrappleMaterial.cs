using UnityEngine;

public class GrappleMaterial : MonoBehaviour
{
    public SpringJoint2D springJoint;
    private Rigidbody2D _rb;

    // Start is called before the first frame update
    private void Start()
    {
        _rb = gameObject.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {

    }

    public void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            springJoint.connectedBody = _rb;
        }
    }
}
