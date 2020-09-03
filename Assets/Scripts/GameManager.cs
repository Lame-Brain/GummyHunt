using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager GAME;
    public GameObject Grass1Prefab, grass2Prefab, grass3Prefab, RockPrefab, ColaPrefab, CherryPrefab, wyrmPrefab;
    [HideInInspector]
    public GameObject[,] tileMap;
    [HideInInspector]
    public int[,] tileAssignment;
    [HideInInspector]
    public GameObject[] grassTrail, colaTree, cherryTree;
    public int mapSize;

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
                tileMap[x, y] = Instantiate(Grass1Prefab, new Vector2(x, y), Quaternion.identity);
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
        while (!foundSnakeSpot)
        {
            int snakeX = Random.Range(5, mapSize - 4), snakeY = Random.Range(5, mapSize - 5);
            foundSnakeSpot = true;
            for(int i = -1; i < 5; i++)
            {
                if (tileAssignment[snakeX+i,snakeY] != 2) foundSnakeSpot = false;
            }
            //if (foundSnakeSpot) Instantiate(wyrmPrefab, new Vector2(snakeX, snakeY), Quaternion.identity);
        }
        Instantiate(wyrmPrefab, new Vector2(2, 2), Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
