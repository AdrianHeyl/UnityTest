using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{

    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 zoom;

    private Vector3 camera_offset;
    private Vector3 model_offset;
    private byte zoom_instance;

    private float rotation_x_offset;
    private const float rotation_x_scale = 2f;
    private const float rotation_x_firstperson = 0f;

    private float rotation_y_offset;
    private const float rotation_y_scale = -2f;
    private const float rotation_y_firstperson = 0f;

    void Start()
    {
        camera_offset = new Vector3(0f, 0f, 0f);
        model_offset = transform.position;
        rotation_y_offset = transform.rotation.eulerAngles.x;
        
        zoom_instance = 0;
    }

    void LateUpdate()
    {
        if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom_instance == 0)
        {
            transform.rotation = Quaternion.Euler(rotation_y_offset, rotation_x_offset, 0f);

            camera_offset = new Vector3(0f, 0f, -2f);
            zoom_instance = 1;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && zoom_instance < 255)
        {
            camera_offset += zoom;
            //transform.position += zoom;
            zoom_instance++;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom_instance == 1)
        {
            transform.rotation = Quaternion.Euler(rotation_y_firstperson, rotation_x_offset, 0f);

            camera_offset = new Vector3(0f, 0f, 0f);
            //transform.position = model_offset;
            zoom_instance = 0;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0 && zoom_instance > 0)
        {
            camera_offset -= zoom;
            //transform.position -= zoom;
            zoom_instance--;
        }

        if (Input.GetMouseButton(1))
        {
            float x = Input.GetAxis("Mouse X");

            float y = Input.GetAxis("Mouse Y");

            transform.rotation = Quaternion.Euler(y * rotation_y_scale, x * rotation_x_scale, 0f);
            //transform.RotateAround(target.position + model_offset, Vector3.up, x * rotation_x_scale);
            //transform.RotateAround(target.position + model_offset, Vector3.right, y * rotation_y_scale);
        }
        else
        {
            if (transform.rotation != target.rotation)
            {
                //transform.RotateAround(target.position + model_offset, Vector3.up, transform.rotation.eulerAngles.y - target.rotation.eulerAngles.y);
            }
        }

        //camera_offset = Quaternion.AngleAxis(transform.rotation.eulerAngles.y - target.rotation.eulerAngles.y, Vector3.up) * camera_offset;
        transform.position = target.position + model_offset + camera_offset;
    }

    public void resetRotationX()
    {
        rotation_x_offset = 0f;
    }
}
