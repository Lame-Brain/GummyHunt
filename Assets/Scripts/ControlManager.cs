using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    public static bool PLAYERHASCONTROL = true;
    public GameObject PointerPanel;
    public Text PointerText;

    private int mouseX, mouseY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!PLAYERHASCONTROL)
        {
            if(PointerPanel.activeSelf) PointerPanel.SetActive(false);
        }
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

            //Set Pointer panel to on, if Player has control
            if (!PointerPanel.activeSelf) PointerPanel.SetActive(true);

            //Get mouse coordinates
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Camera.main.nearClipPlane;
            mouseX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(mousePos).x);
            mouseY = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(mousePos).y);

            //output info to Pointer Text
            string output = "";
            if (mouseX > 0 && mouseX < GameManager.GAME.mapSize && mouseY > 0 && mouseY < GameManager.GAME.mapSize)
            {
                if (GameManager.GAME.tileAssignment[mouseX, mouseY] == 1) output = "ROCK";
                if (GameManager.GAME.tileAssignment[mouseX, mouseY] == 2 || GameManager.GAME.tileAssignment[mouseX, mouseY] == 4 || GameManager.GAME.tileAssignment[mouseX, mouseY] == 5) output = "GRASS";
                if (GameManager.GAME.tileAssignment[mouseX, mouseY] == 3) output = "WORM FOOD";
                if (GameManager.GAME.tileAssignment[mouseX, mouseY] == 6) output = "GUMMY BEAR";
            }
            PointerText.text = output;
        }
    }
}
