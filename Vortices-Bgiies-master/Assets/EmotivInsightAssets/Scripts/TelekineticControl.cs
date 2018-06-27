using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TelekineticControl : MonoBehaviour {

    private Vector3 screenPoint, offset;
    private Vector3 strength = new Vector3(0.0f, 0.0f);
    private Vector3 maxStrength = new Vector3(5f, 5f);
    private Vector3 strengthIncrease = new Vector3(0.05f, 0.05f);

    public Text strengthLevelText;

    const float k_Spring = 50.0f;
    const float k_Damper = 5.0f;
    const float k_Drag = 10.0f;
    const float k_AngularDrag = 5.0f;
    const float k_Distance = 0.2f;
    const bool k_AttachToCenterOfMass = false;

    private RaycastHit hit = new RaycastHit();
    private Rigidbody grabbedObject = null;

    private SpringJoint m_SpringJoint;
    // Use this for initialization
    void Start () {
        
    }

    void OnEnable()
    {
        strengthLevelText.text = "Strength: 0";
    }
	
	// Update is called once per frame
	void Update () {
        if(Input.GetButtonDown("Fire1"))
            StartCoroutine("BuildUpForce");

        var mainCamera = FindCamera();
        if (Input.GetKey(KeyCode.X))
        {
            Debug.Log(strength.x * 10 * Vector3.forward);
            /* This will add force on anything found
            if (
            Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                             mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                             Physics.DefaultRaycastLayers))
            {
                EndDrag();
                hit.rigidbody.AddForce(mainCamera.transform.forward * (5), ForceMode.Impulse);
            }
            */
            if(grabbedObject != null)
            {
                EndDrag();
                grabbedObject.AddForce(mainCamera.transform.forward * (10*strength.x), ForceMode.Impulse);
                grabbedObject = null;
            }                  
            strength = Vector3.zero;
        }        
        // Make sure the user pressed the mouse down
        if (!Input.GetMouseButtonDown(0))
        {
            return;
        }        

        // We need to actually hit an object
        
        if (
            !Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition).origin,
                             mainCamera.ScreenPointToRay(Input.mousePosition).direction, out hit, 100,
                             Physics.DefaultRaycastLayers))
        {
            return;
        }
        // We need to hit a rigidbody that is not kinematic
        if (!hit.rigidbody || hit.rigidbody.isKinematic)
        {
            return;
        }

        if (!m_SpringJoint)
        {
            var go = new GameObject("Rigidbody dragger");
            Rigidbody body = go.AddComponent<Rigidbody>();
            m_SpringJoint = go.AddComponent<SpringJoint>();
            body.isKinematic = true;
            body.drag = 0;
            body.angularDrag = 0.05f;
        }

        m_SpringJoint.transform.position = hit.transform.position;
        m_SpringJoint.anchor = Vector3.zero;

        m_SpringJoint.spring = k_Spring;
        m_SpringJoint.damper = k_Damper;
        m_SpringJoint.maxDistance = k_Distance;
        m_SpringJoint.connectedBody = hit.rigidbody;
        grabbedObject = hit.rigidbody;
        m_SpringJoint.enableCollision = false;

        StartCoroutine("DragObject", hit.distance);
        
    }

    public void Shake()
    {
        CameraShake.Instance.ShakeCamera(strengthIncrease.x, 0.01f);
    }

    private IEnumerator BuildUpForce()
    {
        //build up force
        while (Input.GetButton("Fire1"))
        {
            if (strength.x >= maxStrength.x)
            {
                strength = maxStrength;
            }
            else
            {
                strength = strength + strengthIncrease;
            }
            strengthLevelText.text = "Strength: "+strength.x.ToString();
            yield return null;            
        }
        strength = Vector3.zero;
        strengthLevelText.text = "Strength: " + strength.x.ToString();
    }

    private IEnumerator DragObject(float distance)
    {
        m_SpringJoint.connectedBody.drag = k_Drag;
        m_SpringJoint.connectedBody.angularDrag = k_AngularDrag;        
        var mainCamera = FindCamera();
        while (Input.GetMouseButton(0))
        {
            var ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            m_SpringJoint.transform.position = ray.GetPoint(distance) + Vector3.up*0.5f;
            m_SpringJoint.connectedBody.angularVelocity = Vector3.zero;
            yield return null;
        }
        EndDrag();
    }

    private void EndDrag()
    {
        StopCoroutine("DragObject");
        if (m_SpringJoint.connectedBody)
        {
            m_SpringJoint.connectedBody.drag = 0;
            m_SpringJoint.connectedBody.angularDrag = 0.05f;
            m_SpringJoint.connectedBody = null;
        }        
    }


    private Camera FindCamera()
    {
        if (GetComponent<Camera>())
        {
            return GetComponent<Camera>();
        }

        return Camera.main;
    }
}
