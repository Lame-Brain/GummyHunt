using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    public static bool PLAYERHASCONTROL = true;
    public GameObject PointerPanel;
    public Text PointerText;
    [HideInInspector]
    public GameObject Selected;

    //GummyBear Panel Hookups
    public GameObject GummyBearPanel;
    public Image BearImage;
    public Text NameText, LevelText, XPText, HPText, APText, StrengthText, AccuracyText, SpeedText, FortitudeText, WeaponText, ArmorText;    

    //private float mouseX, mouseY;
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
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            mouseX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(mousePos).x);
            mouseY = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(mousePos).y);

            //output info to Pointer Text
            string output = "";
            if (mouseX >= 0 && mouseX < GameManager.GAME.mapSize && mouseY >= 0 && mouseY < GameManager.GAME.mapSize)
            {
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 1) output = "GRASS";
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 2) output = "ROCK";
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 3) output = "CHERRY";
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 4) output = "COLA";
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 5) output = "GUMMYWORM";
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 6) output = "GUMMYBEAR";

                PointerText.text = output;

                //Click
                if (Input.GetButtonUp("Fire1") && GameManager.ENTITYMAP[mouseX, mouseY] == 6)
                {
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(mouseX, mouseY), Vector2.zero);                    
                    Selected = hit.transform.gameObject;
                    if (!GummyBearPanel.activeSelf) GummyBearPanel.SetActive(true);
                    PLAYERHASCONTROL = false;
                    BearImage.sprite = Selected.GetComponent<SpriteRenderer>().sprite;
                    NameText.text = Selected.GetComponent<GummyBear>().displayName;
                    LevelText.text = "Level: " + Selected.GetComponent<GummyBear>().ExperienceLevel.ToString();
                    XPText.text = "XP: " + Selected.GetComponent<GummyBear>().ExperiencePoints.ToString();
                    HPText.text = "HP: " + (Selected.GetComponent<GummyBear>().HealthPoints - Selected.GetComponent<GummyBear>().Wounds).ToString() + " / " + Selected.GetComponent<GummyBear>().HealthPoints.ToString();
                    APText.text = "HP: " + (Selected.GetComponent<GummyBear>().ActionPoints - Selected.GetComponent<GummyBear>().Fatigue).ToString() + " / " + Selected.GetComponent<GummyBear>().ActionPoints.ToString();
                    StrengthText.text = "STR: " + Selected.GetComponent<GummyBear>().Strength.ToString();
                    AccuracyText.text = "ACC:  " + Selected.GetComponent<GummyBear>().Accuracy.ToString();
                    SpeedText.text = "SPEED: " + Selected.GetComponent<GummyBear>().Speed.ToString();
                    FortitudeText.text = "FORT: " + Selected.GetComponent<GummyBear>().Fortitude.ToString();
                    WeaponText.text = Selected.GetComponent<GummyBear>().weapon + "+" + Selected.GetComponent<GummyBear>().wepBonus.ToString();
                    ArmorText.text = Selected.GetComponent<GummyBear>().armor + "+" + Selected.GetComponent<GummyBear>().armBonus.ToString();
                }
            }
        }
    }

    public void ReturnControltoPlayer()
    {
        PLAYERHASCONTROL = true;
    }
}
