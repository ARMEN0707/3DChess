using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotateAround : MonoBehaviour {

	public Transform target;
	public Vector3 offset;
    public int limit;
	public float sensitivity; 
	public float zoom; 
	public float zoomMax; 
	public float zoomMin; 
	private float y, x;

	void Update ()
	{
        //приближение и отдаление камеры
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            offset.z -= zoom;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            offset.z += zoom;
        }
        offset.z = Mathf.Clamp(offset.z, zoomMax, zoomMin);

        //поворот камеры
        if (Input.GetMouseButton(1))
        {          
            y = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;
            x = transform.localEulerAngles.x + Input.GetAxis("Mouse Y") * sensitivity;
            x = Mathf.Clamp(x, 30, 75);
            transform.localEulerAngles = new Vector3(x, y, 0);            
        }
        transform.position = transform.localRotation * offset + target.position;
    }
}