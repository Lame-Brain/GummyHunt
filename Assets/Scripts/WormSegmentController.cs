using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormSegmentController : MonoBehaviour
{
    public GameObject parentObject;
    public enum direction { west, north, east, south }
    public direction facing;
    public bool isHead, isTail;
    public int MyNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
