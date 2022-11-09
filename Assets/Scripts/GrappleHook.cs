using UnityEngine;

public class GrappleHook : MonoBehaviour
{



    public LineRenderer line;
    public float distance = 1000f;
    public LayerMask mask;
    public float step = 0.02f;
    public GameObject barrelTip;
    private RaycastHit2D _hit;
    private DistanceJoint2D _joint;
    private Vector3 _targetPos;

    // Use this for initialization
    private void Start()
    {
        _joint = GetComponent<DistanceJoint2D>();
        _joint.enabled = false;
        line.enabled = false;
    }

    // Update is called once per frame
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            _targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _targetPos.z = 0;

            _hit = Physics2D.Raycast(transform.position, _targetPos
                                                         - transform.position, distance, mask);

            if (_hit.collider != null && _hit.collider.gameObject.GetComponent<Rigidbody2D>() != null)
            {
                Debug.Log("Hit");
                _joint.enabled = true;
                //	Debug.Log (hit.point - new Vector2(hit.collider.transform.position.x,hit.collider.transform.position.y);
                Vector2 connectPoint =
                    _hit.point - new Vector2(_hit.collider.transform.position.x, _hit.collider.transform.position.y);
                connectPoint.x = connectPoint.x / _hit.collider.transform.localScale.x;
                connectPoint.y = connectPoint.y / _hit.collider.transform.localScale.y;
                Debug.Log(connectPoint);
                _joint.connectedAnchor = connectPoint;

                _joint.connectedBody = _hit.collider.gameObject.GetComponent<Rigidbody2D>();
                //joint.connectedAnchor = hit.point - new Vector2(hit.collider.transform.position.x,hit.collider.transform.position.y);
                _joint.distance = Vector2.Distance(transform.position, _hit.point);

                line.enabled = true;
                line.SetPosition(0, barrelTip.transform.position);
                line.SetPosition(1, _hit.point);

                //line.GetComponent<roperatio>().grabPos = hit.point;


            }
        }
        line.SetPosition(1, _joint.connectedBody.transform.TransformPoint(_joint.connectedAnchor));

        if (Input.GetKey(KeyCode.Mouse1))
        {

            line.SetPosition(0, barrelTip.transform.position);
        }


        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            _joint.enabled = false;
            line.enabled = false;
        }

    }
}

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleHook : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer lineRenderer;
    public DistanceJoint2D joint;

    // Start is called before the first frame update
    void Start()
    {
        joint.enabled = false;
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            
            Vector2 mousePos = (Vector2)mainCamera.ScreenToWorldPoint(Input.mousePosition);
            lineRenderer.SetPosition(0, mousePos);
            lineRenderer.SetPosition(1, transform.position);
            joint.anchor = transform.position;
            joint.connectedAnchor = mousePos;
            joint.enabled = true;
            lineRenderer.enabled = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            joint.enabled = false;
            lineRenderer.enabled = false;
        }
        if (joint.enabled)
        {
            lineRenderer.SetPosition(1, transform.position);
        }
    }
}
*/
