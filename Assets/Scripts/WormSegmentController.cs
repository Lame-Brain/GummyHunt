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

    public void FixedUpdate()
    {
        this.GetComponent<Animator>().SetBool("Hurt", false);
    }
}
