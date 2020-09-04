using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour
{
    public int turnsToRegrow, turnsSinceEaten = 0;
    public bool eaten = false;
    private float bitOfRandomness;

    //Called when object is instantiated
    private void Awake()
    {
        Randomize(25f);
    }

    private void Randomize(float amount)
    {
        bitOfRandomness = Random.Range(0f, amount);
    }

    public void PassTurn()
    {
        if (eaten)
        {
            turnsSinceEaten++;
            if (turnsSinceEaten > turnsToRegrow + bitOfRandomness)
            {
                eaten = false;
                GetComponent<SpriteRenderer>().enabled = true;
            }
        }
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (!eaten)
        {
            if (other.tag == "SnakeSegment")
            {
                eaten = true; GetComponent<SpriteRenderer>().enabled = false;
                other.GetComponentInParent<WormMotor>().GrowWorm();
            }
        }

    }

}
