using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{ 
    public float speed;
    private float xSpeed, ySpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + xSpeed, Camera.main.transform.position.y + ySpeed, Camera.main.transform.position.z);
    }

    public void MoveCameraUp()
    {
        ySpeed = speed;
    }
    public void StopCameraUp()
    {
        ySpeed = 0;
    }

    public void MoveCameraDown()
    {
        ySpeed = -speed;
    }
    public void StopCameraDown()
    {
        ySpeed = 0;
    }

    public void MoveCameraLeft()
    {
        xSpeed = -speed;
    }
    public void StopCameraLeft()
    {
        xSpeed = 0;
    }

    public void MoveCameraRight()
    {
        xSpeed = speed;
    }
    public void StopCameraRight()
    {
        xSpeed = 0;
    }
}
