using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailControl : MonoBehaviour
{
    public int barren_lifetime, halfgrown_lifetime;

    [SerializeField]
    private int counter = 0;
    private bool active = false;

    public void ToggleActive()
    {
        active = !active;
    }

    public void SetActive(bool set)
    {
        active = set;
    }

    public void PassTime()
    {
        if(active) counter++;
        if(counter > barren_lifetime && GameManager.GAME.tileAssignment[(int)transform.position.x, (int)transform.position.y] != 5)
        {
            GetComponent<SpriteRenderer>().sprite = GameManager.GAME.GrassHalfGrowth;
            GameManager.GAME.tileAssignment[(int)transform.position.x, (int)transform.position.y] = 5;
        }
        if(counter > barren_lifetime + halfgrown_lifetime)
        {
            counter = 0;
            GameManager.GAME.tileAssignment[(int)transform.position.x, (int)transform.position.y] = 2;
            transform.position = new Vector2(0, 0);
            GetComponent<SpriteRenderer>().enabled = false;
            active = false;
        }
    }
}
