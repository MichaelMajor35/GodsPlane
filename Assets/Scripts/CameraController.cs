using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/** 
    Guide For Use:

        Control:            KeyCode         Mouse Control       (alt/F to find and replace)
            
            Pan Up:         KeyCode.W       GetMouseButtonDown(0)   GetMouseButton(0)
            Pan Down:       KeyCode.S       
            Pan Right:      KeyCode.D       
            Pan Left:       KeyCode.A       
            Rotate Right:   KeyCode.E       GetMouseButtonDown(2)   GetMouseButton(2)
            Rotate Left:    KeyCode.Q       
            Zoom In:        KeyCode.R       mouseScrollDelta.y
            Zoom Out:       KeyCode.F       
            Stop Follow:    KeyCode.Escape  


        Camera Follow Selection:

            public void OnMouseDown()
            {
                CameraController.instance.followSelection = transform;
            }

**/

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    public Transform cameraTransform;//For Zoom
    public Transform followSelection;

    public float panSpeed;
    public float fastPanSpeed;
    public float zoomSpeed;
    public float rotationSpeed;
    public float lerpTime;
    public float movementSpeed;

    public Vector3 zoomAmount;
    public Vector3 newZoom;
    public Vector3 newPosition;
    public Quaternion newRotation;

    public Vector3 dragStartPosition;
    public Vector3 dragCurrentPosition;
    public Vector3 rotStartPosition;
    public Vector3 rotCurrentPosition;
    
    
    // Start is called before the first frame update

    void Start()
    {   
        instance = this;
        newPosition = transform.position;
        newRotation = transform.rotation;
        newZoom = cameraTransform.localPosition;

        zoomAmount.y = -zoomSpeed;
        zoomAmount.z = zoomSpeed;
    }

    // Update is called once per frame
    
    void Update()
    {
        if(followSelection != null)
        {
            transform.position = followSelection.position;
        }
        else
        {
            HandleMouseInput();
            HandleMovementInput();
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            followSelection = null;
        }
    }

    void HandleMouseInput()
    {
        if(Input.mouseScrollDelta.y != 0)
        {
            newZoom += Input.mouseScrollDelta.y * zoomAmount * zoomSpeed;
        }

        if(Input.GetMouseButtonDown(0))
        {
            Plane plane = new Plane(Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragStartPosition = ray.GetPoint(entry);
            }
        }
        if(Input.GetMouseButton(0))
        {
            Plane plane = new Plane (Vector3.up, Vector3.zero);

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            float entry;

            if(plane.Raycast(ray, out entry))
            {
                dragCurrentPosition = ray.GetPoint(entry);

                newPosition = transform.position + dragStartPosition - dragCurrentPosition;
            }
        }
        
        
        if(Input.GetMouseButtonDown(2))
        {
            rotStartPosition = Input.mousePosition;
        }
        if(Input.GetMouseButton(2))
        {
            rotCurrentPosition = Input.mousePosition;

            Vector3 difference = rotStartPosition - rotCurrentPosition;

            rotStartPosition = rotCurrentPosition;

            newRotation *= Quaternion.Euler(Vector3.up * (-difference.x / 5f));
        }
    }

    void HandleMovementInput()
    {
        if(Input.GetKey(KeyCode.LeftShift))
        {
            movementSpeed = fastPanSpeed;
        }
        else
        {
            movementSpeed = panSpeed;
        }
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            newPosition += (transform.forward * movementSpeed);
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            newPosition += (transform.forward * -movementSpeed);
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            newPosition += (transform.right * movementSpeed);
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            newPosition += (transform.right * -movementSpeed);
        }

        if (Input.GetKey(KeyCode.Q))
        {
            newRotation *= Quaternion.Euler(Vector3.up * rotationSpeed);
        }        
        if (Input.GetKey(KeyCode.E))
        {
            newRotation *= Quaternion.Euler(Vector3.up * -rotationSpeed);
        }
        
        
        /*
        
        if(input.getkey(keycode.r))
        {
            
            newzoom += zoomamount;
        }
        if(input.getkey(keycode.f))
        {
            newzoom -= zoomamount;
        }
        */

        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * lerpTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, Time.deltaTime * lerpTime);
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newZoom, Time.deltaTime * lerpTime);
    }
}
