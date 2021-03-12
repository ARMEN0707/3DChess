using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateAround : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
	public float sensitivity; 
	public float limit = 80; 
	public float zoom; 
	public float zoomMax; 
	public float zoomMin; 
	private float y, x;

	void Update ()
	{
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            offset.z -= zoom;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            offset.z += zoom;
        }
        offset.z = Mathf.Clamp(offset.z, zoomMax, zoomMin);

        if (Input.GetMouseButton(1))
        {          
            x = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            y += Input.GetAxis("Mouse Y") * sensitivity;
            y = Mathf.Clamp(y, -limit, limit);
            transform.localEulerAngles = new Vector3(-y, x, 0);
            
        }
        transform.position = transform.localRotation * offset + target.position;
    }
}