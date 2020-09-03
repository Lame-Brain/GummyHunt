using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    public static bool PLAYERHASCONTROL = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PLAYERHASCONTROL)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                Camera.main.GetComponent<CameraControl>().MoveCameraUp();
            }
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                Camera.main.GetComponent<CameraControl>().StopCameraUp();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                Camera.main.GetComponent<CameraControl>().MoveCameraDown();
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                Camera.main.GetComponent<CameraControl>().StopCameraDown();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                Camera.main.GetComponent<CameraControl>().MoveCameraLeft();
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                Camera.main.GetComponent<CameraControl>().StopCameraLeft();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                Camera.main.GetComponent<CameraControl>().MoveCameraRight();
            }
            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                Camera.main.GetComponent<CameraControl>().StopCameraRight();
            }
        }
    }
}
