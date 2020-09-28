using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ControlManager : MonoBehaviour
{
    public static bool PLAYERHASCONTROL = true;
    public GameObject PointerPanel;
    public Text PointerText;
    [HideInInspector]
    public GameObject Selected, SelectedGO = null, validIco = null, invalidIco = null, target = null;
    public GameObject BracketGO, ValidGO, InvalidGO;

    //Screen Hookups
    public GameObject GummyBearPanel, HelpPanelObject, QuitScreenPanel, KilledTheWormPanel;

    //GummyBear Panel Hookups
    public Image BearImage;
    public Text NameText, LevelText, XPText, HPText, APText, StrengthText, AccuracyText, SpeedText, FortitudeText, WeaponText, ArmorText;    

    //private float mouseX, mouseY;
    private int mouseX, mouseY;

    //Modes
    private bool moveMode = false, attackMode = false, validMove = false, validAttack = false; 

    // Start is called before the first frame update
    void Start()
    {
        validIco = Instantiate(ValidGO, new Vector2(-100, -100), Quaternion.identity);
        invalidIco = Instantiate(InvalidGO, new Vector2(-100, -100), Quaternion.identity);
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
            //Help Screen
            if (Input.GetKeyUp(KeyCode.F1)) ActivateHelpScreen();

            //Quit Screen
            if (Input.GetKeyUp(KeyCode.Escape)) ToggleQuitScreen();

            //Camera  Control
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
            {
                Camera.main.GetComponent<CameraControl>().MoveCameraUp();
            }
            if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.W))
            {
                Camera.main.GetComponent<CameraControl>().StopCameraUp();
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                Camera.main.GetComponent<CameraControl>().MoveCameraDown();
            }
            if (Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.S))
            {
                Camera.main.GetComponent<CameraControl>().StopCameraDown();
            }
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                Camera.main.GetComponent<CameraControl>().MoveCameraLeft();
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.A))
            {
                Camera.main.GetComponent<CameraControl>().StopCameraLeft();
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                Camera.main.GetComponent<CameraControl>().MoveCameraRight();
            }
            if (Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.D))
            {
                Camera.main.GetComponent<CameraControl>().StopCameraRight();
            }

            if (Input.GetButton("Fire2"))
            {
                float MouseMag = Camera.main.GetComponent<CameraControl>().speed * 2.0f;
                if (Input.GetAxisRaw("Mouse Y") < 0) Camera.main.GetComponent<CameraControl>().ScrollCameraY(MouseMag); //moving Up?
                if (Input.GetAxisRaw("Mouse Y") > 0) Camera.main.GetComponent<CameraControl>().ScrollCameraY(-MouseMag); //moving Down?
                if (Input.GetAxisRaw("Mouse Y") == 0) Camera.main.GetComponent<CameraControl>().ScrollCameraY(0);
                if (Input.GetAxisRaw("Mouse X") < 0) Camera.main.GetComponent<CameraControl>().ScrollCameraX(MouseMag); //moving Left?
                if (Input.GetAxisRaw("Mouse X") > 0) Camera.main.GetComponent<CameraControl>().ScrollCameraX(-MouseMag); //moving Right?
                if (Input.GetAxisRaw("Mouse X") == 0) Camera.main.GetComponent<CameraControl>().ScrollCameraX(0);
            }
            if (Input.GetButtonUp("Fire2")) { Camera.main.GetComponent<CameraControl>().ScrollCameraX(0); Camera.main.GetComponent<CameraControl>().ScrollCameraY(0); }

            //Set Pointer panel to on, if Player has control
            if (!PointerPanel.activeSelf) PointerPanel.SetActive(true);

            //Get mouse coordinates
            Vector3 mousePos = Input.mousePosition;
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            mouseX = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(mousePos).x);
            mouseY = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(mousePos).y);

            //output info to Pointer Text and cursor
            string output = "";
            if (mouseX >= 0 && mouseX < GameManager.GAME.mapSize && mouseY >= 0 && mouseY < GameManager.GAME.mapSize)
            {
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 1) output = "GRASS";
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 2) output = "ROCK";
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 3) output = "CHERRY";
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 4) output = "COLA";
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 5)
                {
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(mouseX, mouseY), Vector2.zero);
                    GameObject gWorm = hit.transform.gameObject;
                    output = "GUMMYWORM " + gWorm.GetComponent<WormSegmentController>().parentObject.GetComponent<WormMotor>().Health.ToString();
                }
                if (GameManager.ENTITYMAP[mouseX, mouseY] == 6) output = "GUMMYBEAR";
                if (GameManager.VISIBLE[mouseX, mouseY] == 0) output = "FOG";

                //Output to Text panel
                PointerText.text = output;

                //Output to cursor stuff
                if(moveMode) //For moving
                {
                    float moveRange = Vector2.Distance(new Vector2(Selected.transform.position.x, Selected.transform.position.y), new Vector2(mouseX, mouseY));
                    if (GameManager.ENTITYMAP[mouseX, mouseY] != 2 && GameManager.ENTITYMAP[mouseX, mouseY] != 5 && GameManager.ENTITYMAP[mouseX, mouseY] != 6 
                        && moveRange <= Selected.GetComponent<GummyBear>().Speed && ((moveRange*2)+Selected.GetComponent<GummyBear>().Fatigue) <= Selected.GetComponent<GummyBear>().ActionPoints && GameManager.VISIBLE[mouseX, mouseY] == 1)
                    {
                        validIco.transform.position = new Vector2(mouseX, mouseY);
                        invalidIco.transform.position = new Vector2(-100, -100);
                        validMove = true;
                    }
                    else
                    {
                        validIco.transform.position = new Vector2(-100, -100);
                        invalidIco.transform.position = new Vector2(mouseX, mouseY);
                        validMove = false;
                    }
                }

                if(attackMode)//for Attacking
                {
                    float attackRange = Vector2.Distance(new Vector2(Selected.transform.position.x, Selected.transform.position.y), new Vector2(mouseX, mouseY));
                    if (GameManager.ENTITYMAP[mouseX, mouseY] == 5 && Selected.GetComponent<GummyBear>().Fatigue+2 <= Selected.GetComponent<GummyBear>().ActionPoints && GameManager.VISIBLE[mouseX,mouseY] == 1) //if square is visible and is a worm
                    {
                        validIco.transform.position = new Vector2(mouseX, mouseY);
                        invalidIco.transform.position = new Vector2(-100, -100);
                        validAttack = true;
                    }
                    else //square is not visible, or does not have a worm
                    {
                        validIco.transform.position = new Vector2(-100, -100); 
                        invalidIco.transform.position = new Vector2(mouseX, mouseY);
                        validAttack = false;
                    }
                }
                if(!moveMode && !attackMode)
                {
                    validIco.transform.position = new Vector2(-100, -100);
                    invalidIco.transform.position = new Vector2(-100, -100);
                }

                //Click
                if (Input.GetButtonUp("Fire1") && GameManager.ENTITYMAP[mouseX, mouseY] == 6 && !EventSystem.current.IsPointerOverGameObject()) //Click on Gummy Bear                
                {
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(mouseX, mouseY), Vector2.zero);                    
                    Selected = hit.transform.gameObject;
                    if (SelectedGO != null) SelectedGO.transform.position = new Vector2(mouseX, mouseY);
                    if (SelectedGO == null) SelectedGO = Instantiate(BracketGO, new Vector2(mouseX, mouseY), Quaternion.identity);
                    if (!GummyBearPanel.activeSelf) GummyBearPanel.SetActive(true);
                    UpdateGummyPanel();
                }

                if(Input.GetButtonUp("Fire1") && !EventSystem.current.IsPointerOverGameObject() && !moveMode && !attackMode && GameManager.ENTITYMAP[mouseX, mouseY] != 6) //unselect
                {
                    UnSelect();
                }

                if (Input.GetButtonUp("Fire1") && moveMode && validMove && !EventSystem.current.IsPointerOverGameObject()) //click to move to target square
                {
                    GameManager.ENTITYMAP[(int)Selected.transform.position.x, (int)Selected.transform.position.y] = 1;
                    Selected.GetComponent<GummyBear>().Fatigue += (int)Vector2.Distance(new Vector2(Selected.transform.position.x, Selected.transform.position.y), new Vector2(mouseX, mouseY)) * 2;
                    Selected.transform.parent.position = new Vector2(mouseX, mouseY);
                    SelectedGO.transform.position = new Vector2(mouseX, mouseY);
                    GameManager.ENTITYMAP[(int)Selected.transform.position.x, (int)Selected.transform.position.y] = 6;
                    moveMode = false;
                    Camera.main.GetComponent<CameraControl>().MoveCamera2Point(mouseX, mouseY);
                    UpdateGummyPanel();
                }

                if (Input.GetButtonUp("Fire1") && attackMode && validAttack && !EventSystem.current.IsPointerOverGameObject()) //click to attack a worm
                {
                    Selected.GetComponent<GummyBear>().Fatigue += 2;
                    attackMode = false;
                    RaycastHit2D hit = Physics2D.Raycast(new Vector2(mouseX, mouseY), Vector2.zero);
                    target = hit.transform.gameObject; target.GetComponent<Animator>().SetBool("Hurt", true);
                    target = target.GetComponent<WormSegmentController>().parentObject;
                    target.GetComponent<WormMotor>().Health += -10;
                    if (target.GetComponent<WormMotor>().Health <= 0)
                    {
                        target.GetComponent<WormMotor>().KillWorm();
                    }
                    target = null;
                    Camera.main.GetComponent<CameraControl>().MoveCamera2Point(mouseX, mouseY);
                    UpdateGummyPanel();
                }


            }
            if (Input.GetButtonUp("Fire1") && !EventSystem.current.IsPointerOverGameObject())
            {
                if (mouseX < 0 || mouseX >= GameManager.GAME.mapSize || mouseY < 0 || mouseY >= GameManager.GAME.mapSize) //outside bounds, deselect.
                {
                    UnSelect();
                }
            }

        }
    }

    public void UnSelect()
    {
        Selected = null;
        if (GummyBearPanel.activeSelf) GummyBearPanel.SetActive(false);
        if (SelectedGO != null) Destroy(SelectedGO);
        moveMode = false;
        attackMode = false;
    }

    public void UpdateGummyPanel()
    {
        BearImage.sprite = Selected.GetComponent<SpriteRenderer>().sprite;
        NameText.text = Selected.GetComponent<GummyBear>().displayName;
        LevelText.text = "Level: " + Selected.GetComponent<GummyBear>().ExperienceLevel.ToString();
        XPText.text = "XP: " + Selected.GetComponent<GummyBear>().ExperiencePoints.ToString();
        HPText.text = "HP: " + (Selected.GetComponent<GummyBear>().HealthPoints - Selected.GetComponent<GummyBear>().Wounds).ToString() + " / " + Selected.GetComponent<GummyBear>().HealthPoints.ToString();
        APText.text = "AP: " + (Selected.GetComponent<GummyBear>().ActionPoints - Selected.GetComponent<GummyBear>().Fatigue).ToString() + " / " + Selected.GetComponent<GummyBear>().ActionPoints.ToString();
        StrengthText.text = "STR: " + Selected.GetComponent<GummyBear>().Strength.ToString();
        AccuracyText.text = "ACC:  " + Selected.GetComponent<GummyBear>().Accuracy.ToString();
        SpeedText.text = "SPEED: " + Selected.GetComponent<GummyBear>().Speed.ToString();
        FortitudeText.text = "FORT: " + Selected.GetComponent<GummyBear>().Fortitude.ToString();
        WeaponText.text = Selected.GetComponent<GummyBear>().weapon; // + "+" + Selected.GetComponent<GummyBear>().wepBonus.ToString();
        ArmorText.text = Selected.GetComponent<GummyBear>().armor; // + "+" + Selected.GetComponent<GummyBear>().armBonus.ToString();
    }

    public void ReturnControltoPlayer()
    {
        PLAYERHASCONTROL = true;
        moveMode = false;
        if (SelectedGO != null) Destroy(SelectedGO);
    }

    public void MoveMode()
    {
        moveMode = !moveMode;
    }
    public void AttackMode()
    {
        attackMode = !attackMode;
    }

    public void ActivateHelpScreen()
    {
        HelpPanelObject.SetActive(!HelpPanelObject.activeSelf);
    }

    public void KilledAGummyWorm()
    {
        KilledTheWormPanel.SetActive(true);
        PLAYERHASCONTROL = false;
    }

    public void ToggleQuitScreen()
    {
        QuitScreenPanel.SetActive(!QuitScreenPanel.activeSelf);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SelectBearLeft()
    {
        GameManager.GAME.GummyBearIndex--;
        if (GameManager.GAME.GummyBearIndex < 0) GameManager.GAME.GummyBearIndex = GameManager.GAME.GummyBearList.Count-1;
        Selected = GameManager.GAME.GummyBearList[GameManager.GAME.GummyBearIndex];
        int selectedX = (int)GameManager.GAME.GummyBearList[GameManager.GAME.GummyBearIndex].transform.position.x, selectedY = (int)GameManager.GAME.GummyBearList[GameManager.GAME.GummyBearIndex].transform.position.y;
        if (SelectedGO != null) SelectedGO.transform.position = new Vector2(selectedX, selectedY);
        if (SelectedGO == null) SelectedGO = Instantiate(BracketGO, new Vector2(selectedX, selectedY), Quaternion.identity);
        if (!GummyBearPanel.activeSelf) GummyBearPanel.SetActive(true);
        Camera.main.GetComponent<CameraControl>().MoveCamera2Point(selectedX, selectedY);
        UpdateGummyPanel();
    }
    public void SelectBearRight()
    {
        GameManager.GAME.GummyBearIndex++;
        if (GameManager.GAME.GummyBearIndex > GameManager.GAME.GummyBearList.Count-1) GameManager.GAME.GummyBearIndex = 0;
        Selected = GameManager.GAME.GummyBearList[GameManager.GAME.GummyBearIndex];
        int selectedX = (int)GameManager.GAME.GummyBearList[GameManager.GAME.GummyBearIndex].transform.position.x, selectedY = (int)GameManager.GAME.GummyBearList[GameManager.GAME.GummyBearIndex].transform.position.y;
        if (SelectedGO != null) SelectedGO.transform.position = new Vector2(selectedX, selectedY);
        if (SelectedGO == null) SelectedGO = Instantiate(BracketGO, new Vector2(selectedX, selectedY), Quaternion.identity);
        if (!GummyBearPanel.activeSelf) GummyBearPanel.SetActive(true);
        Camera.main.GetComponent<CameraControl>().MoveCamera2Point(selectedX, selectedY);
        UpdateGummyPanel();
    }
}
