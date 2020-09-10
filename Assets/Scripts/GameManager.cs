using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME;
    public GameObject GrassPrefab, trailPrefab, RockPrefab, ColaPrefab, CherryPrefab, wyrmPrefab;
    public GameObject[] GummyPrefab;
    public Sprite GrassFullGrowth, GrassHalfGrowth, GrassDead;
    [HideInInspector]
    public GameObject[,] tileMap;
    //[HideInInspector]
    public int[,] tileAssignment;
    [HideInInspector]
    public GameObject[] grassTrail, colaTree, cherryTree;
    public int mapSize;
    public int TrailObjectCache;

    private GameObject[] trail;
    [SerializeField]
    private int trailCount = 0;

    //TEST until turn structure is in place
    private float delay = 50f, counter = 0f;

    //Awake
    void Awake()
    {
        GAME = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Initialize Arrays
        tileMap = new GameObject[mapSize, mapSize];
        tileAssignment = new int[mapSize, mapSize];
        trail = new GameObject[TrailObjectCache];

        //Set Map Borders
        for(int i = 0; i < mapSize; i++)
        {
            tileMap[0, i] = Instantiate(RockPrefab, new Vector2(0, i), Quaternion.identity);
            tileMap[i, 0] = Instantiate(RockPrefab, new Vector2(i, 0), Quaternion.identity);
            tileMap[mapSize - 1, i] = Instantiate(RockPrefab, new Vector2(mapSize - 1, i), Quaternion.identity);
            tileMap[i, mapSize - 1] = Instantiate(RockPrefab, new Vector2(i, mapSize - 1), Quaternion.identity);
            tileAssignment[0, i] = 1;
            tileAssignment[i, 0] = 1;
            tileAssignment[mapSize - 1, i] = 1;
            tileAssignment[i, mapSize - 1] = 1;
        }

        //Fill in the grass
        for(int y = 1; y < mapSize-1; y++)
        {
            for(int x = 1; x < mapSize-1; x++)
            {
                tileMap[x, y] = Instantiate(GrassPrefab, new Vector2(x, y), Quaternion.identity);
                tileAssignment[x, y] = 2;
            }
        }
                
        //place rocks 
        for(int i = 0; i < mapSize; i++)
        {
            int x = Random.Range(1, mapSize - 1), y = Random.Range(1, mapSize - 1);
            tileMap[x, y-1] = Instantiate(RockPrefab, new Vector2(x, y-1), Quaternion.identity); tileAssignment[x, y - 1] = 1;
            tileMap[x-1, y] = Instantiate(RockPrefab, new Vector2(x-1, y), Quaternion.identity); tileAssignment[x - 1, y] = 1;
            tileMap[x, y] = Instantiate(RockPrefab, new Vector2(x, y), Quaternion.identity); tileAssignment[x, y] = 1;
            tileMap[x, y+1] = Instantiate(RockPrefab, new Vector2(x+1, y), Quaternion.identity); tileAssignment[x, y + 1] = 1;
            tileMap[x+1, y] = Instantiate(RockPrefab, new Vector2(x, y+1), Quaternion.identity); tileAssignment[x + 1, y] = 1;
            //Debug.Log(x + ", " + y);
        }

        //place powerups
        int c = 0;
        while(c < mapSize)
        {
            int x = Random.Range(1, (mapSize-1)), y = Random.Range(1, (mapSize - 1)), t = Random.Range(0,4);
            if (tileAssignment[x,y] == 2)
            {
                c++;
                tileAssignment[x, y] = 3;                
                if (t == 0 || t == 2) tileMap[x, y] = Instantiate(ColaPrefab, new Vector2(x, y), Quaternion.identity);
                if (t == 1 || t == 3) tileMap[x, y] = Instantiate(CherryPrefab, new Vector2(x, y), Quaternion.identity);
            }
        }

        //place Snake
        bool foundSnakeSpot = false;
        GameObject snek;
        while (!foundSnakeSpot)
        {
            int snakeX = Random.Range(5, mapSize - 4), snakeY = Random.Range(5, mapSize - 5);
            foundSnakeSpot = true;
            for(int i = -1; i < 5; i++)
            {
                if (tileAssignment[snakeX+i,snakeY] != 2) foundSnakeSpot = false;
            }
            if (foundSnakeSpot)
            {
                snek = Instantiate(wyrmPrefab, new Vector2(0, 0), Quaternion.identity);                
                snek.GetComponent<WormMotor>().BirthWorm(snakeX, snakeY); //replace 2,2 with snakeX and snakeY
            }
            
        }

        //Fill Trail Cache
        for(int i = 0; i < TrailObjectCache; i++)
        {
            trail[i] = Instantiate(trailPrefab, new Vector2(0, 0), Quaternion.identity);
            trail[i].GetComponent<SpriteRenderer>().enabled = false;
        }

        //place the Gummies
        Instantiate(GummyPrefab[Random.Range(0, 12)], new Vector2(2, 2),Quaternion.identity);

    }

    // Update is called once per frame
    void Update()
    {
        counter++;
        if(counter > delay)
        {
            counter = 0;
            PassTurn();
        }
    }

    public void PassTurn()
    {
        //Move Gummyworms
        GameObject[] Snakes = GameObject.FindGameObjectsWithTag("Snake");
        for (int i = 0; i < Snakes.Length; i++)
        {
            Snakes[i].GetComponent<WormMotor>().MoveWorm();
            
            int x = (int)Snakes[i].GetComponent<WormMotor>().HeadPos().x, y = (int)Snakes[i].GetComponent<WormMotor>().HeadPos().y;
            if (tileAssignment[x,y] == 2 || tileAssignment[x, y] == 3) //GummyWorm is moving over a tile with full growth
            {
                tileAssignment[x, y] = 4;
                trail[trailCount].transform.position = Snakes[i].GetComponent<WormMotor>().HeadPos();
                trail[trailCount].GetComponent<SpriteRenderer>().sprite = GrassDead;
                trail[trailCount].GetComponent<SpriteRenderer>().enabled = true;
                trail[trailCount].GetComponent<TrailControl>().SetActive(true);
                trailCount++;
                if (trailCount > (TrailObjectCache - 1)) trailCount = 0;
                if(tileAssignment[x, y] == 3) //Gummyworm has eaten a powerup
                {

                }
            }
        }
        //Age Trail objects
        for(int i = 0; i < TrailObjectCache; i++)
        {
            trail[i].GetComponent<TrailControl>().PassTime();
        }
    }
}

/*TILE ASSIGNMENT MAP
 * 1 = Rock Tile
 * 2 = Grass Tile (full growth)
 * 3 = PowerUp
 * 4 = Trail Object (dead grass)
 * 5 = Trail Object (half-growth)
 */