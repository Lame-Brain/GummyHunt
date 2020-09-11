using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GummyBear : MonoBehaviour
{
    public string name;
    public int Strength, Accuracy, Speed, Fortitude, ActionPoints, HealthPoints, Fatigue, Wounds;
    public int ExperiencePoints, ExperienceLevel;

    //Panel Hookups
    public GameObject GummyBearPanel;
    public Sprite BearImage;
    public Text StrengthText, AccuracyText, SpeedText, FortitudeText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectBear()
    {
        ControlManager.PLAYERHASCONTROL = false;
        if(!GummyBearPanel.activeSelf) GummyBearPanel.SetActive(true);
        BearImage = transform.GetComponent<SpriteRenderer>().sprite;
        StrengthText.text = "STRENGTH " + Strength.ToString();
    }
}
